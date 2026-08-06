# Guía de Estilos — Midnight Modern

Sistema de diseño derivado de `qcmAuditoriasIatf`. Usar como referencia canónica para nuevos proyectos Blazor en el mismo ecosistema.

---

## 1. Variables CSS (Design Tokens)

Declarar en `:root` del `app.css` de cada proyecto:

```css
:root {
    /* Paleta principal */
    --color-primary: #060B29;
    --color-primary-hover: #1e3a8a;
    --color-accent-vibrant: #3b82f6;
    --color-primary-gradient: linear-gradient(135deg, #060B29 0%, #1c3375 100%);
    --color-secondary: #0f172a;

    /* Texto */
    --text: #1e293b;
    --text-muted: #64748b;

    /* Fondos y superficies */
    --bg: #f8fafc;
    --surface: #ffffff;
    --border: #e2e8f0;
    --input-bg: #f1f5f9;
    --placeholder: #94a3b8;

    /* Layout */
    --sidebar-w: 260px;
    --navbar-h: 64px;

    /* Border radius */
    --radius: 16px;
    --radius-sm: 10px;

    /* Sombras */
    --shadow-sm: 0 1px 3px 0 rgb(0 0 0 / 0.1), 0 1px 2px -1px rgb(0 0 0 / 0.1);
    --shadow-md: 0 4px 6px -1px rgb(0 0 0 / 0.1), 0 2px 4px -2px rgb(0 0 0 / 0.1);
    --shadow-lg: 0 20px 25px -5px rgb(0 0 0 / 0.1), 0 8px 10px -6px rgb(0 0 0 / 0.1);
    --shadow-primary: 0 10px 15px -3px rgba(6, 11, 41, 0.3);
    --focus: 0 0 0 4px rgba(6, 11, 41, 0.15);
}
```

---

## 2. Tipografía

**Stack:** `system-ui, -apple-system, "Segoe UI", Roboto, Helvetica, Arial, sans-serif`  
**Suavizado:** `-webkit-font-smoothing: antialiased`

### Escala tipográfica

| Token | Valor | Uso |
|---|---|---|
| xs | `0.72rem` | Labels de sección, etiquetas uppercase |
| sm | `0.78–0.82rem` | Pills, badges, table headers |
| base-sm | `0.85–0.88rem` | Notas, metadatos, subtítulos |
| base | `0.92–1rem` | Cuerpo principal |
| md | `1.05–1.15rem` | Valores de summary, títulos menores |
| lg | `1.08rem` | Encabezados de panel (`panel-head h2`) |
| xl | `1.2–1.25rem` | Títulos de empty state |
| hero | `clamp(1.9rem, 3vw, 2.7rem)` | Títulos de hero (h1) |

### Pesos

| Peso | Clase | Uso |
|---|---|---|
| 600 | `.fw-semibold` | Texto regular de énfasis |
| 700 | `.fw-bold` | Labels, botones, form labels |
| 800 | `.fw-extrabold` | Valores métricos, títulos de celda, h1/h2 |

### Labels uppercase

```css
.label-upper {
    font-size: 0.72rem;
    font-weight: 700;
    letter-spacing: 0.08em;
    text-transform: uppercase;
    color: var(--text-muted);
}
```

---

## 3. Estructura de Página

Todas las páginas siguen el mismo patrón:

```
.xxx-page          → flex column, gap: 1.25rem
  .xxx-hero        → banner oscuro (gradient), border-radius: 28px
    .xxx-kicker    → pill badge sobre el título
    h1             → título principal (clamp font-size)
    .meta-chip(s)  → chips de contexto en el hero
    .hero-actions  → botones en el hero
  .xxx-metrics     → grid de 4 (o 6) metric-cards
  .panel-card(s)   → contenido principal con tablas, formularios, listas
```

### Página raíz

```css
.page-name {
    display: flex;
    flex-direction: column;
    gap: 1.25rem;
}
```

---

## 4. Sección Hero

```css
.xxx-hero {
    display: flex;              /* o grid si hay sidebar de acciones */
    justify-content: space-between;
    align-items: flex-start;
    gap: 1rem;
    padding: 1.6rem 1.75rem;
    border-radius: 28px;
    background: linear-gradient(135deg, rgba(6,11,41,0.97) 0%, rgba(28,51,117,0.97) 100%);
    color: #fff;
    box-shadow: 0 24px 45px rgba(15,23,42,0.16);
}
```

### Kicker (badge sobre el título)

```css
.xxx-kicker {
    display: inline-flex;
    align-items: center;
    padding: 0.38rem 0.78rem;
    border-radius: 999px;
    background: rgba(255,255,255,0.10);
    color: rgba(255,255,255,0.86);
    font-size: 0.82rem;
    font-weight: 700;
    margin-bottom: 0.9rem;
}
```

### Chips de metadatos en hero

```css
.meta-chip {
    padding: 0.45rem 0.8rem;
    border-radius: 999px;
    background: rgba(255,255,255,0.10);
    color: rgba(255,255,255,0.86);
    font-size: 0.88rem;
}
```

### Botones en hero

```css
.hero-btn {
    min-height: 46px;
    padding: 0.78rem 1.15rem;
    border-radius: 16px;
    background: rgba(255,255,255,0.10);
    color: #fff;
    font-weight: 700;
    border: 1px solid rgba(255,255,255,0.08);
    transition: all 0.18s ease;
    display: inline-flex;
    align-items: center;
    justify-content: center;
}

.hero-btn:hover {
    background: rgba(255,255,255,0.16);
    color: #fff;
}

/* Variante primaria (CTA) */
.hero-btn.primary {
    background: linear-gradient(135deg, #3b82f6 0%, #1d4ed8 100%);
    border-color: transparent;
    box-shadow: 0 12px 24px rgba(59,130,246,0.25);
}
```

---

## 5. Panel Card

Contenedor principal de contenido (blanco, sobre el fondo gris):

```css
.panel-card {
    padding: 1.25rem;
    border-radius: 24px;
    background: rgba(255,255,255,0.94);
    border: 1px solid rgba(226,232,240,0.95);
    box-shadow: 0 14px 30px rgba(15,23,42,0.06);
}

.panel-head {
    display: flex;
    align-items: flex-start;
    justify-content: space-between;
    gap: 1rem;
    margin-bottom: 1rem;
}

.panel-head h2 {
    margin: 0;
    font-size: 1.08rem;
    color: #0f172a;
    font-weight: 800;
}

.panel-subtitle {
    margin: 0.3rem 0 0;
    color: #64748b;
    font-size: 0.9rem;
}

/* Badge de conteo en panel */
.panel-badge {
    padding: 0.38rem 0.75rem;
    border-radius: 999px;
    background: #eff6ff;
    color: #1d4ed8;
    font-size: 0.82rem;
    font-weight: 700;
    white-space: nowrap;
}
```

---

## 6. Metric Card

```css
.metric-card {
    padding: 1.15rem 1.2rem;
    border-radius: 22px;
    background: rgba(255,255,255,0.94);
    border: 1px solid rgba(226,232,240,0.95);
    box-shadow: 0 14px 30px rgba(15,23,42,0.06);
}

.metric-label {
    color: #64748b;
    font-size: 0.9rem;
    font-weight: 700;
    margin-bottom: 0.55rem;
}

.metric-value {
    color: #0f172a;
    font-size: 2rem;
    font-weight: 800;
    line-height: 1;
    margin-bottom: 0.35rem;
}

.metric-note {
    color: #64748b;
    font-size: 0.85rem;
}
```

Grid de métricas (4 columnas en escritorio):

```css
.xxx-metrics {
    display: grid;
    grid-template-columns: repeat(4, minmax(0, 1fr));
    gap: 1rem;
}
```

---

## 7. Tablas

```css
.table-wrap {
    width: 100%;
    overflow-x: auto;
}

.data-table {
    width: 100%;
    min-width: 760px;         /* ajustar según columnas */
    border-collapse: separate;
    border-spacing: 0;
}

.data-table thead th {
    padding: 0.95rem 1rem;
    font-size: 0.82rem;
    text-transform: uppercase;
    letter-spacing: 0.04em;
    color: #64748b;
    background: #f8fafc;
    border-bottom: 1px solid #e2e8f0;
}

.data-table thead th:first-child { border-top-left-radius: 16px; }
.data-table thead th:last-child  { border-top-right-radius: 16px; }

.data-table tbody td {
    padding: 1rem;
    border-bottom: 1px solid #eef2f7;
    vertical-align: top;     /* o middle según contenido */
    background: #fff;
    color: #1e293b;
}

.data-table tbody tr:hover td {
    background: #fbfdff;
}

/* Celda con título + subtítulo */
.cell-title {
    font-weight: 800;
    color: #0f172a;
}

.cell-subtitle {
    margin-top: 0.18rem;
    color: #64748b;
    font-size: 0.85rem;
    line-height: 1.35;;
}
```

### Botón de tabla

```css
.table-btn {
    min-height: 38px;
    padding: 0.55rem 0.9rem;
    border: 1px solid #cbd5e1;
    border-radius: 12px;
    background: #fff;
    color: #0f172a;
    font-weight: 700;
    transition: all 0.18s ease;
}

.table-btn:hover { background: #f8fafc; border-color: #94a3b8; }

/* Variantes */
.table-btn-primary {
    background: linear-gradient(135deg, #3b82f6 0%, #1d4ed8 100%);
    border-color: transparent; color: #fff;
    box-shadow: 0 10px 18px rgba(59,130,246,0.18);
}

.table-btn-success {
    background: linear-gradient(135deg, #10b981 0%, #059669 100%);
    border-color: transparent; color: #fff;
    box-shadow: 0 10px 18px rgba(16,185,129,0.18);
}

.table-btn-danger {
    background: linear-gradient(135deg, #ef4444 0%, #dc2626 100%);
    border-color: transparent; color: #fff;
    box-shadow: 0 10px 18px rgba(239,68,68,0.18);
}

.table-btn-info {
    background: linear-gradient(135deg, #06b6d4 0%, #0891b2 100%);
    border-color: transparent; color: #fff;
    box-shadow: 0 10px 18px rgba(6,182,212,0.18);
}
```

---

## 8. Pills / Badges de Estado

Base común para todos los pills:

```css
.status-pill,
.tipo-pill,
.flow-pill,
.validacion-pill {
    display: inline-flex;
    align-items: center;
    justify-content: center;
    min-height: 34px;
    padding: 0.35rem 0.75rem;
    border-radius: 999px;
    font-size: 0.78rem;
    font-weight: 800;
    white-space: nowrap;
}
```

### Status (estados de auditoría / hallazgo)

| Clase | Bg | Color | Semántica |
|---|---|---|---|
| `.status-pill.open` | `#fef2f2` | `#dc2626` | Abierto / Pendiente crítico |
| `.status-pill.progress` | `#fff7ed` | `#ea580c` | En progreso |
| `.status-pill.closed` | `#ecfdf5` | `#059669` | Cerrado / Completado |
| `.status-pill.blocked` | `#fef2f2` | `#dc2626` | Bloqueado |

### Tipo de hallazgo

| Clase | Bg | Color |
|---|---|---|
| `.tipo-pill.major` | `#fef2f2` | `#dc2626` |
| `.tipo-pill.minor` | `#fff7ed` | `#ea580c` |
| `.tipo-pill.neutral` | `#eff6ff` | `#2563eb` |

### Validación / Flujo

| Clase | Bg | Color |
|---|---|---|
| `.validacion-pill.pending` | `#fff7ed` | `#b45309` |
| `.validacion-pill.approved` | `#ecfdf5` | `#059669` |
| `.validacion-pill.rejected` | `#fef2f2` | `#dc2626` |
| `.validacion-pill.neutral` | `#f1f5f9` | `#475569` |

### ID Pill (azul, para IDs / conteos)

```css
.id-pill {
    display: inline-flex;
    align-items: center;
    justify-content: center;
    min-height: 34px;
    padding: 0.35rem 0.75rem;
    border-radius: 999px;
    background: #eff6ff;
    color: #1d4ed8;
    font-weight: 800;
    font-size: 0.84rem;
}
```

---

## 9. Formularios

```css
.form-label {
    font-weight: 700;
    color: #334155;
}

.form-control,
.form-select {
    min-height: 48px;
    border-radius: 14px;
    border: 1px solid #dbe3ee;
    background: #fff;
    box-shadow: none;
}

.form-control:focus,
.form-select:focus {
    border-color: #60a5fa;
    box-shadow: 0 0 0 0.18rem rgba(59,130,246,0.14);
}

.field-help {
    margin-top: 0.35rem;
    color: #64748b;
    font-size: 0.84rem;
    line-height: 1.35;
}

.section-divider {
    height: 1px;
    background: #e2e8f0;
    margin: 1rem 0;
}
```

### Cajas de aviso / notice

```css
/* Informativo — naranja/advertencia */
.notice-box {
    padding: 0.95rem 1rem;
    border-radius: 18px;
    background: #fff7ed;
    border: 1px solid #fed7aa;
    color: #9a3412;
    font-size: 0.9rem;
    line-height: 1.45;
}

/* Informativo — azul/neutral */
.notice-box.info {
    background: #eff6ff;
    border-color: #bfdbfe;
    color: #1d4ed8;
}
```

---

## 10. Empty States

```css
/* Completo (página vacía) */
.empty-state {
    padding: 2rem 1rem;
    text-align: center;
}

/* Compacto (inline dentro de panel) */
.empty-state.compact {
    padding: 1rem;
    border-radius: 18px;
    background: #f8fafc;
    border: 1px dashed #cbd5e1;
    color: #64748b;
}

.empty-title {
    font-size: 1.25rem;
    font-weight: 800;
    color: #0f172a;
    margin-bottom: 0.45rem;
}

.empty-text {
    color: #64748b;
    max-width: 520px;
    margin: 0 auto;
}
```

---

## 11. Skeleton / Loading

```css
@keyframes shimmer {
    0%   { background-position: 100% 0; }
    100% { background-position: 0 0; }
}

.skeleton {
    border-radius: 24px;
    background: linear-gradient(90deg, #e5e7eb 25%, #f3f4f6 37%, #e5e7eb 63%);
    background-size: 400% 100%;
    animation: shimmer 1.4s ease infinite;
}

/* Alturas comunes */
.skeleton-hero   { height: 220px; }
.skeleton-card   { height: 130px; }
.skeleton-table  { height: 360px; }
.skeleton-metric { height: 120px; }
```

---

## 12. Transición Estándar

Usar siempre el mismo valor para mantener coherencia:

```css
transition: all 0.18s ease;
```

Aplicar a botones, links, tarjetas con hover, nav items.

---

## 13. Breakpoints Responsivos

| Nombre | Ancho | Cambio principal |
|---|---|---|
| Desktop grande | > 1200px | Layout completo |
| Desktop | > 1100px | Grid de métricas 4 cols |
| Tablet | ≤ 1100px | Métricas a 2 cols, grids a 1 col, hero stack |
| Móvil | ≤ 768px | Panel padding reducido, botones full-width |
| Móvil pequeño | ≤ 640px | Métricas a 1 col, todo apilado |

```css
@media (max-width: 1100px) {
    .xxx-metrics { grid-template-columns: repeat(2, minmax(0, 1fr)); }
    .xxx-hero    { flex-direction: column; }
}

@media (max-width: 768px) {
    .xxx-hero, .panel-card { padding: 1rem; }
    .hero-actions          { width: 100%; flex-direction: column; }
    .hero-btn              { width: 100%; }
    .panel-head            { flex-direction: column; align-items: flex-start; }
}

@media (max-width: 640px) {
    .xxx-metrics { grid-template-columns: 1fr; }
}
```

---

## 14. Sidebar / Navegación

```css
/* Gradiente del sidebar */
.nav-menu-root {
    background: linear-gradient(180deg, #060B29 0%, #1c3375 100%);
}

/* Item activo — barra indicadora izquierda */
.nav-link.active::before {
    content: "";
    position: absolute;
    left: 0;
    top: 50%;
    transform: translateY(-50%);
    width: 4px;
    height: 22px;
    background: #3b82f6;
    border-radius: 0 4px 4px 0;
    box-shadow: 0 0 8px rgba(59,130,246,0.45);
}

/* Sección del usuario en sidebar */
.nav-user-box {
    margin: auto 0.85rem 0;
    padding: 1rem;
    border-radius: 18px;
    background: rgba(255,255,255,0.08);
    border: 1px solid rgba(255,255,255,0.10);
    backdrop-filter: blur(6px);
}
```

---

## 15. Convenciones de Nomenclatura

| Patrón | Uso | Ejemplo |
|---|---|---|
| `.xxx-page` | Contenedor raíz de la página | `.auditorias-page` |
| `.xxx-hero` | Banner superior oscuro | `.auditorias-hero` |
| `.xxx-kicker` | Badge sobre el h1 | `.auditorias-kicker` |
| `.xxx-metrics` | Grid de metric-cards | `.auditorias-metrics` |
| `.xxx-loading` | Contenedor de skeletons | `.auditorias-loading` |
| `.xxx-skeleton` | Bloque shimmer | `.auditorias-skeleton` |
| `.panel-card` | Panel de contenido genérico | — |
| `.table-btn` | Botón en filas de tabla | — |
| `.hero-btn` | Botón en sección hero | — |
| `.status-pill` | Badge de estado | — |
| `.empty-state` | Estado vacío | — |

---

## 16. Colores Semánticos (resumen rápido)

| Semántica | Bg | Texto | Uso |
|---|---|---|---|
| Danger / Error | `#fef2f2` | `#dc2626` | Abierto, rechazado, bloqueado, major |
| Warning / En progreso | `#fff7ed` | `#ea580c` / `#b45309` | En progreso, pendiente, minor |
| Success | `#ecfdf5` | `#059669` | Cerrado, aprobado |
| Info / Primary | `#eff6ff` | `#1d4ed8` | IDs, neutral info, azul |
| Neutral | `#f1f5f9` | `#475569` | Sin estado, bloqueado leve |
| Purple | `#f5f3ff` | `#7c3aed` | Formación/training |
| Cyan | `#ecfeff` | `#0891b2` | Observador, info secundaria |
| Green (líneas) | `#f0fdf4` | `#166534` | Secciones de líneas de producción |
