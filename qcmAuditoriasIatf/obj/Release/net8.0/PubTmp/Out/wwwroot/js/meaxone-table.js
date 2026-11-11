/* ══════════════════════════════════════════════════════════════
   MEAX One — mo-table.js
   Filtros por columna + paginación para tablas .mo-table,
   sin dependencias. Funciona sobre HTML ya renderizado
   (server-side con Razor, o estático), así que sirve igual
   para tablas armadas en C#/Razor, PHP, Node, etc.

   MARCADO ESPERADO:
   <div class="mo-table-wrap" data-mo-table data-page-size="10">
     <table class="mo-table">
       <thead>
         <tr> ...encabezados normales... </tr>
         <tr class="mo-table-filter-row">
           <th><input class="mo-th-filter" data-mo-filter-col="0" placeholder="Filtrar…"></th>
           <th><select class="mo-th-filter" data-mo-filter-col="2"><option value="">Todos</option>...</select></th>
           <th></th> <!-- columna sin filtro: <th> vacío -->
         </tr>
       </thead>
       <tbody> ...filas... </tbody>
     </table>
   </div>
   <div class="mo-table-pagination" data-mo-pagination></div>

   No requiere llamar nada manualmente: se auto-inicializa sola.

   Por qué NO basta con DOMContentLoaded:
   Muchas pantallas de Blazor Server muestran primero un esqueleto de
   carga ("cargando") y solo agregan la tabla real al DOM más tarde,
   cuando el circuito de SignalR responde con los datos — después de
   que DOMContentLoaded ya se disparó. Además, Blazor puede reemplazar
   filas de una tabla ya inicializada en cualquier momento (filtros del
   servidor, aprobar/rechazar un registro, etc.), también sin disparar
   un nuevo DOMContentLoaded. En ambos casos, una tabla que no se
   inicializó a tiempo se queda con las cajitas de "Filtrar…" sin
   ningún listener — el síntoma es que se ve todo normal pero escribir
   en el filtro no hace nada.

   Por eso este script observa TODO el documento con un MutationObserver
   y reinicializa las tablas [data-mo-table] presentes cada vez que el
   DOM cambia (con un pequeño debounce), sin importar cuándo ni cómo
   Blazor las haya agregado o modificado.
══════════════════════════════════════════════════════════════ */
(function () {
    'use strict';

    // Mientras es mayor a 0, las mutaciones que haga este script sobre el DOM
    // (agregar/quitar la fila de "sin resultados", reconstruir los botones de
    // paginación) no vuelven a disparar una reinicialización — evita que el
    // propio script entre en bucle reaccionando a sus propios cambios. Es un
    // contador (no un booleano) porque varias llamadas guardadas pueden
    // solaparse (p. ej. initAll() reinicializando varias tablas, cada una
    // agregando su propia fila vacía) y cada una libera su parte por separado.
    var mutationGuardDepth = 0;

    function withMutationGuard(fn) {
        mutationGuardDepth++;
        fn();
        // Se libera en una macrotarea (setTimeout) para que el callback del
        // MutationObserver (entregado como microtarea, que corre antes que
        // cualquier setTimeout) alcance a ver el contador todavía activo y
        // descarte las mutaciones que acabamos de hacer nosotros.
        setTimeout(function () {
            mutationGuardDepth = Math.max(0, mutationGuardDepth - 1);
        }, 0);
    }

    function initTable(wrap) {
        // Si esta tabla ya se había inicializado antes (por ejemplo, Blazor
        // acaba de cambiarle las filas), primero se limpia el estado anterior
        // (listeners, fila de "sin resultados") para reconstruirlo desde
        // cero contra el DOM actual.
        if (wrap._moTableCleanup) {
            wrap._moTableCleanup();
        }

        var table = wrap.querySelector('table.mo-table');
        if (!table) return;
        var tbody = table.querySelector('tbody');
        if (!tbody) return;

        var allRows = Array.prototype.slice.call(tbody.querySelectorAll('tr'));
        var filterInputs = Array.prototype.slice.call(wrap.querySelectorAll('.mo-th-filter'));
        var paginationEl = wrap.nextElementSibling && wrap.nextElementSibling.hasAttribute('data-mo-pagination')
            ? wrap.nextElementSibling
            : wrap.parentElement.querySelector('[data-mo-pagination]');

        var pageSize = parseInt(wrap.getAttribute('data-page-size'), 10) || 10;
        var currentPage = 1;

        // Fila de "sin resultados" (se inserta una vez y se muestra/oculta).
        var emptyRow = document.createElement('tr');
        var colCount = table.querySelectorAll('thead tr:first-child th').length || 1;
        emptyRow.innerHTML = '<td class="mo-table-empty" colspan="' + colCount + '">No se encontraron resultados con esos filtros.</td>';
        emptyRow.style.display = 'none';
        withMutationGuard(function () { tbody.appendChild(emptyRow); });

        // Valor que representa a una celda en un filtro de lista desplegable: el
        // atributo data-mo-value de la celda si existe (útil cuando la celda mezcla
        // varios textos), o su texto visible.
        function cellValue(cell) {
            if (!cell) return '';
            var v = cell.getAttribute('data-mo-value');
            return (v !== null ? v : cell.textContent).replace(/\s+/g, ' ').trim();
        }

        // Los <select data-mo-auto> arman sus opciones con los valores distintos que
        // realmente aparecen en la columna, así la lista siempre coincide con la tabla.
        function populateAutoSelects() {
            withMutationGuard(function () {
                filterInputs.forEach(function (input) {
                    if (input.tagName !== 'SELECT' || !input.hasAttribute('data-mo-auto')) return;

                    var col = parseInt(input.getAttribute('data-mo-filter-col'), 10);
                    var seen = {};
                    var values = [];

                    allRows.forEach(function (row) {
                        var v = cellValue(row.children[col]);
                        if (v && !seen[v]) {
                            seen[v] = true;
                            values.push(v);
                        }
                    });

                    values.sort(function (a, b) {
                        return a.localeCompare(b, 'es', { numeric: true, sensitivity: 'base' });
                    });

                    var previous = input.value;

                    input.innerHTML = '';

                    var all = document.createElement('option');
                    all.value = '';
                    all.textContent = 'Todos';
                    input.appendChild(all);

                    values.forEach(function (v) {
                        var opt = document.createElement('option');
                        opt.value = v;
                        opt.textContent = v;
                        input.appendChild(opt);
                    });

                    if (previous && seen[previous]) input.value = previous;
                });
            });
        }

        function rowMatchesFilters(row) {
            for (var i = 0; i < filterInputs.length; i++) {
                var input = filterInputs[i];
                var val = (input.value || '').trim().toLowerCase();
                if (!val) continue;
                var col = parseInt(input.getAttribute('data-mo-filter-col'), 10);
                var cell = row.children[col];
                if (input.tagName === 'SELECT') {
                    if (cellValue(cell).toLowerCase() !== val) return false;
                } else {
                    var cellText = cell ? cell.textContent.trim().toLowerCase() : '';
                    if (cellText.indexOf(val) === -1) return false;
                }
            }
            return true;
        }

        function getFiltered() {
            return allRows.filter(rowMatchesFilters);
        }

        function render() {
            // renderPagination() reconstruye por completo el <div data-mo-pagination>
            // (borra y vuelve a crear sus botones) cada vez que se llama, incluyendo
            // cuando el usuario da clic en un botón de página. Sin este seguro, esa
            // reconstrucción se veía como un cambio de Blazor y disparaba una
            // reinicialización que reiniciaba currentPage a 1 — deshaciendo el clic.
            withMutationGuard(function () {
                var filtered = getFiltered();
                var totalPages = Math.max(1, Math.ceil(filtered.length / pageSize));
                if (currentPage > totalPages) currentPage = totalPages;
                if (currentPage < 1) currentPage = 1;

                var start = (currentPage - 1) * pageSize;
                var end = start + pageSize;
                var pageRows = filtered.slice(start, end);

                allRows.forEach(function (r) { r.classList.add('mo-row-hidden'); });
                pageRows.forEach(function (r) { r.classList.remove('mo-row-hidden'); });

                emptyRow.style.display = filtered.length === 0 ? '' : 'none';

                if (paginationEl) renderPagination(paginationEl, filtered.length, totalPages);
            });
        }

        function renderPagination(el, totalResults, totalPages) {
            var start = totalResults === 0 ? 0 : (currentPage - 1) * pageSize + 1;
            var end = Math.min(currentPage * pageSize, totalResults);

            el.innerHTML = '';

            var summary = document.createElement('div');
            summary.className = 'mo-pg-summary';
            summary.innerHTML = totalResults === 0
                ? 'Sin resultados'
                : 'Mostrando <b>' + start + '–' + end + '</b> de <b>' + totalResults + '</b>';
            el.appendChild(summary);

            var right = document.createElement('div');
            right.className = 'mo-pg-right';

            // Selector de tamaño de página
            var sizeWrap = document.createElement('div');
            sizeWrap.className = 'mo-pg-size';
            var label = document.createElement('label');
            label.textContent = 'Filas por página';
            var select = document.createElement('select');
            [5, 10, 25, 50].forEach(function (n) {
                var opt = document.createElement('option');
                opt.value = n; opt.textContent = n;
                if (n === pageSize) opt.selected = true;
                select.appendChild(opt);
            });
            select.addEventListener('change', function () {
                pageSize = parseInt(select.value, 10);
                currentPage = 1;
                render();
            });
            sizeWrap.appendChild(label);
            sizeWrap.appendChild(select);
            right.appendChild(sizeWrap);

            // Botones de página
            var pagesWrap = document.createElement('div');
            pagesWrap.className = 'mo-pg-pages';

            function makeBtn(html, page, opts) {
                opts = opts || {};
                var btn = document.createElement('button');
                btn.type = 'button';
                btn.className = 'mo-pg-btn' + (opts.active ? ' active' : '');
                btn.innerHTML = html;
                btn.disabled = !!opts.disabled;
                btn.addEventListener('click', function () {
                    currentPage = page;
                    render();
                });
                return btn;
            }

            pagesWrap.appendChild(makeBtn('&lsaquo;', currentPage - 1, { disabled: currentPage <= 1 }));

            var pagesToShow = [];
            for (var p = 1; p <= totalPages; p++) {
                if (p === 1 || p === totalPages || Math.abs(p - currentPage) <= 1) pagesToShow.push(p);
            }
            var lastShown = 0;
            pagesToShow.forEach(function (p) {
                if (lastShown && p - lastShown > 1) {
                    var dots = document.createElement('span');
                    dots.className = 'mo-pg-ellipsis';
                    dots.textContent = '…';
                    pagesWrap.appendChild(dots);
                }
                pagesWrap.appendChild(makeBtn(String(p), p, { active: p === currentPage }));
                lastShown = p;
            });

            pagesWrap.appendChild(makeBtn('&rsaquo;', currentPage + 1, { disabled: currentPage >= totalPages }));
            right.appendChild(pagesWrap);

            el.appendChild(right);
        }

        var filterListeners = [];
        filterInputs.forEach(function (input) {
            var evt = input.tagName === 'SELECT' ? 'change' : 'input';
            var handler = function () {
                currentPage = 1;
                render();
            };
            input.addEventListener(evt, handler);
            filterListeners.push({ input: input, evt: evt, handler: handler });
        });

        wrap._moTableCleanup = function () {
            filterListeners.forEach(function (l) {
                l.input.removeEventListener(l.evt, l.handler);
            });
            if (emptyRow.parentNode) {
                withMutationGuard(function () { emptyRow.parentNode.removeChild(emptyRow); });
            }
            wrap._moTableCleanup = null;
        };

        populateAutoSelects();
        render();
    }

    function initAll() {
        Array.prototype.slice.call(document.querySelectorAll('[data-mo-table]')).forEach(initTable);
    }

    function scrollTo(id) {
        var el = document.getElementById(id);
        if (el) el.scrollIntoView({ behavior: 'smooth', block: 'start' });
    }

    window.moTable = { init: initTable, initAll: initAll, scrollTo: scrollTo };

    document.addEventListener('DOMContentLoaded', initAll);

    function startWatching() {
        var pending = null;

        new MutationObserver(function () {
            if (mutationGuardDepth > 0) return;
            clearTimeout(pending);
            pending = setTimeout(initAll, 40);
        }).observe(document.body, { childList: true, subtree: true });
    }

    if (document.body) {
        startWatching();
    } else {
        document.addEventListener('DOMContentLoaded', startWatching);
    }
})();
