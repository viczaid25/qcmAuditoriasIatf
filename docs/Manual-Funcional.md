# Manual Funcional del Sistema de Auditorías IATF

**Sistema:** qcmAuditoriasIatf
**Empresa:** Mitsubishi Electric Automotive de México, S.A. de C.V.
**Ámbito:** Gestión de auditorías internas IATF (planeación, checklist, hallazgos, validación SGC/QMS e informe final)

---

## 1. ¿Qué es el sistema?

qcmAuditoriasIatf es la aplicación con la que el equipo de Calidad planea y ejecuta las auditorías internas IATF de la planta. Permite:

- Planear una auditoría y asignar los procesos que se van a auditar, con sus auditores.
- Llenar el checklist de preguntas de cada proceso durante la auditoría.
- Generar un **hallazgo** (no conformidad) cuando una pregunta del checklist no se cumple.
- Dar seguimiento al hallazgo hasta su cierre: validación de SGC, análisis de causa (5 porqués o acción correctiva según el tipo de hallazgo), evidencia de solución y verificación final de QMS.
- Consultar un **informe de auditoría** con toda la información recopilada, descargable en Excel o PDF.
- Enviar notificaciones automáticas por correo en cada paso relevante del proceso.

El sistema se accede con el usuario y contraseña de red de Active Directory (el mismo que se usa para iniciar sesión en la computadora); no existe un usuario o contraseña independiente del sistema.

---

## 2. Perfiles y niveles de acceso

El sistema no maneja roles cerrados de "menú"; casi todas las pantallas están disponibles para cualquier persona que inicie sesión, pero **ciertas acciones** dentro de cada pantalla solo las puede ejecutar la persona correcta. Los criterios que se repiten en todo el sistema son:

| Perfil / condición | Cómo se determina | Qué habilita |
|---|---|---|
| **Usuario autenticado** | Cualquier persona que inició sesión con su cuenta de red | Consultar la mayoría de las pantallas, ver auditorías, hallazgos, catálogos |
| **SGC / Administrador** | Su usuario de red está dado de alta en el catálogo de perfiles con `Perfil = "SGC"` o `"Administrador"` y `Activo = Sí` | Validar hallazgos, 5 porqués y acciones correctivas; cerrar hallazgos (bandeja QMS); eliminar auditorías; editar las conclusiones de una auditoría; capturar el "Seguimiento para el informe" |
| **Auditor líder de la auditoría** | Es la persona registrada como "Auditor líder" al crear la auditoría | Editar las conclusiones de la auditoría |
| **Auditor principal del proceso** | Es la persona asignada como "Auditor principal" en ese proceso dentro de la auditoría | Editar y reenviar a validación un hallazgo que SGC rechazó |
| **Responsable de cierre del hallazgo** | Es la persona registrada como responsable de cerrar ese hallazgo específico | Capturar los 5 porqués o la acción correctiva, y subir la evidencia de solución |

> **Nota:** Las pantallas del catálogo de **Procesos** (`/procesos`, alta y edición de proceso) no tienen ninguna restricción de acceso: cualquier persona que entre a esas direcciones puede modificarlas, a diferencia de casi todo lo demás en el sistema. Es importante tenerlo presente si se requiere restringir quién administra el catálogo de procesos.

---

## 3. Mapa de pantallas

### 3.1 Inicio y pendientes personales

| Pantalla | Para qué sirve |
|---|---|
| **Inicio** (`/`) | Panel con conteos generales: auditorías, procesos, auditores activos, hallazgos abiertos/en proceso/cerrados, y accesos directos a "Nueva auditoría", "Nuevo proceso" y "Alta auditor". |
| **Mis cosas** (`/mis-cosas`) | Bandeja personal de trabajo: "Mis pendientes" (lista priorizada de tareas propias), "Mis procesos asignados" (como auditor principal, segundo auditor u observador) y "Mis hallazgos" (como responsable o responsable de cierre). Se puede filtrar por vencidos, por esta semana o por texto. |

### 3.2 Auditorías

| Pantalla | Para qué sirve |
|---|---|
| **Auditorías** (`/auditorias`) | Listado histórico con métricas (total, vigentes, finalizadas, del mes) y tabla filtrable. Botón "Nueva auditoría". |
| **Nueva auditoría** (`/auditorias/nueva`) | Alta de una auditoría: datos generales + selección de los procesos que se van a incluir. |
| **Detalle de auditoría** (`/auditorias/{id}`) | Panel central de la auditoría: estatus, conclusiones, métricas, tabla de procesos con acciones "Asignar/Editar" y "Checklist", botón para agregar procesos y (solo SGC) eliminar la auditoría. |
| **Asignar/editar proceso de la auditoría** (`/auditorias/{id}/procesos/agregar` y `/auditorias/{id}/procesos/{auditoriaProcesoId}`) | Define quién audita cada proceso (auditor principal, segundo auditor, observador), la fecha programada y, si aplica, la línea de producción. |
| **Checklist del proceso** (`/auditorias/{id}/procesos/{auditoriaProcesoId}/checklist`) | Captura de resultados (Cumple / No Cumple / No Aplica / Pendiente) por cada pregunta del proceso, con evidencia. Desde aquí se genera un hallazgo cuando una pregunta no se cumple. |
| **Calendario de la auditoría** (`/auditorias/{id}/calendario`) | Vista de calendario mensual con las fechas programadas de cada proceso; permite exportar la vista como imagen o PDF. |
| **Informe de auditoría** (`/auditorias/{id}/informe`) | Reporte final en pantalla, descargable en Excel o PDF (ver sección 4.5). |

### 3.3 Hallazgos

| Pantalla | Para qué sirve |
|---|---|
| **Nuevo hallazgo desde checklist** (`/auditorias/{id}/procesos/{auditoriaProcesoId}/checklist/{preguntaId}/hallazgo/nuevo`) | Formulario para registrar la no conformidad detectada en una pregunta del checklist. |
| **Detalle del hallazgo** (`/auditorias/{id}/hallazgos/{hallazgoId}`) | Pantalla central del hallazgo: validación SGC, descripción/evidencia, 5 porqués o acción correctiva según el tipo, evidencia de solución, estatus de cierre y (solo SGC) el "Seguimiento para el informe". |
| **Editar hallazgo rechazado** (`/auditorias/{id}/hallazgos/{hallazgoId}/editar`) | Solo visible para el auditor principal del proceso; permite corregir un hallazgo que SGC rechazó y reenviarlo a validación. |
| **Hallazgos de la auditoría** (`/auditorias/{id}/hallazgos`) | Bandeja de hallazgos de una auditoría específica, con métricas y filtros. |
| **Hallazgos global** (`/hallazgos`) | Bandeja de hallazgos de todas las auditorías — **solo muestra los que ya pasaron la validación de SGC** (`Aprobado`). |

### 3.4 Interfaces de validación (SGC / QMS)

| Pantalla | Para qué sirve | Acceso |
|---|---|---|
| **Validación SGC de hallazgos** (`/interfaces/sgc/hallazgos`) | Aprobar o rechazar hallazgos recién creados. | Solo SGC/Administrador |
| **Validación SGC de 5 porqués** (`/interfaces/sgc/cinco-porques`) | Aprobar o rechazar el análisis de 5 porqués (hallazgos NC Menor). | Solo SGC/Administrador |
| **Validación SGC de acciones correctivas** (`/interfaces/sgc/acciones-correctivas`) | Aprobar o rechazar el archivo de acción correctiva (hallazgos NC Mayor). | Solo SGC/Administrador |
| **Verificación QMS de hallazgos** (`/interfaces/qms/verificacion-hallazgos`) | Revisar la evidencia de solución y cerrar o rechazar el hallazgo. | Solo SGC/Administrador |

### 3.5 Catálogos

| Pantalla | Para qué sirve | Acceso |
|---|---|---|
| **Auditores** (`/auditores`, alta y edición) | Catálogo de personas que pueden ser auditor, observador o estar en entrenamiento; permite buscar y prellenar datos desde el directorio de Active Directory. | Usuario autenticado |
| **Procesos** (`/procesos`, alta y edición) | Catálogo de procesos auditables (código, nombre, área, unidad de negocio, línea, descripción, activo/inactivo). | **Sin restricción de acceso** |
| **Preguntas de checklist** (`/procesos/checklist-preguntas`, alta y edición) | Administración del banco de preguntas por proceso, con enlace opcional a una cláusula IATF del catálogo. | Usuario autenticado |
| **Importar checklist desde Excel** (`/procesos/importar-checklist`) | Carga masiva de preguntas de checklist desde un archivo Excel con el formato "MX-6100-F6". | Usuario autenticado |

---

## 4. Flujos de trabajo paso a paso

### 4.1 Crear y planear una auditoría

1. En **Auditorías** o en el Inicio, dar clic en **"Nueva auditoría"**.
2. Llenar los datos generales: Fecha inicio, Fecha fin, Tipo (Interna/Externa/Cliente), Estatus (Planeada/En Proceso/Cerrada — inicia en "Planeada"), Auditor líder, Objetivo, Alcance, Criterios y Métodos.
3. Seleccionar los procesos a incluir. El sistema los presenta en dos grupos:
   - **Procesos generales**: cualquier proceso activo sin unidades de negocio.
   - **Producción / unidad**: el proceso combinado "MX-7000-D1 / MX-6000-D1" (Producción / Calidad Proceso), que se despliega en tarjetas por unidad de negocio (Alternador, Marchas, Car Mechatronics, EPS, Multimedia, Car Electronics). Cada tarjeta de producción permite además agregar una o varias **líneas de producción** con su propia información de contexto.
   - Cada tarjeta se puede "Capturar" para registrar: persona(s) a auditar, reclamos, resultados de auditorías previas, seguimiento de acciones correctivas IATF y las cláusulas TOP 3 de NC Mayor/Menor previas.
4. Dar clic en **"Guardar auditoría"**. El sistema crea la auditoría y un registro por cada proceso (o línea) seleccionado, sin auditor asignado ni fecha programada todavía.
5. En el **Detalle de la auditoría**, cada proceso muestra el botón **"Asignar"** (luego "Editar"). Ahí se define: Auditor principal (obligatorio), Segundo auditor y Observador (opcionales, deben ser personas distintas entre sí), Fecha programada (debe estar dentro del rango de la auditoría) y Línea (obligatoria solo si el proceso pertenece a una unidad de negocio).
6. Al guardar la asignación, el sistema crea automáticamente las filas del checklist ("Pendiente") para todas las preguntas activas de ese proceso, listas para llenarse.
7. En el **Calendario de la auditoría** se puede visualizar el mes con todas las fechas programadas, ver qué procesos aún no tienen fecha ("Procesos pendientes por programar"), y exportar la vista como imagen o PDF.

### 4.2 Llenar el checklist y generar un hallazgo

1. Desde el Detalle de la auditoría, dar clic en **"Checklist"** sobre el proceso correspondiente.
2. Por cada pregunta activa se selecciona un resultado: **Pendiente / Cumple / No Cumple / No Aplica**, y se puede capturar Evidencia.
3. Dar clic en **"Guardar checklist"**. El sistema solo guarda las preguntas que realmente se modificaron, para no sobrescribir el trabajo de otra persona que esté llenando el mismo checklist al mismo tiempo.
4. Cuando una pregunta queda en **"No Cumple"** y todavía no tiene un hallazgo relacionado, aparece el botón **"Crear hallazgo"**; si ya tiene uno, el botón cambia a **"Ver hallazgo"**.
5. Al dar clic en "Crear hallazgo" se abre el formulario de alta: Fecha compromiso (por defecto, 7 días después), Tipo de hallazgo (NC Mayor / NC Menor / Observación de Mejora), Responsable de finalizar el hallazgo (responsable de cierre), Descripción, **Evidencia (obligatoria)** y Justificación de la no conformidad.
6. Al guardar, el hallazgo queda con estatus **"Pendiente SGC"** y se notifica por correo a QMS.

### 4.3 Ciclo de vida completo de un hallazgo

1. **Creación** → estatus "Pendiente SGC", validación SGC "Pendiente". Mientras no se valide, el detalle del hallazgo muestra un aviso bloqueante y oculta las secciones de análisis y evidencia.
2. **Validación SGC del hallazgo** (`/interfaces/sgc/hallazgos`): SGC revisa descripción, evidencia y justificación, y puede:
   - **Aprobar** → validación SGC "Aprobado", estatus "Abierto". Se notifica a los auditores del proceso y al responsable de cierre.
   - **Rechazar** (requiere comentario) → validación SGC "Rechazado", el hallazgo permanece "Pendiente SGC". Se notifica a los auditores del proceso.
   - Un hallazgo rechazado **solo puede corregirlo el auditor principal del proceso**, desde "Editar hallazgo", donde ve el motivo del rechazo, corrige los datos y elige **"Guardar cambios"** o **"Reenviar a validación"** (esto último vuelve a poner el hallazgo en "Pendiente SGC" y notifica a QMS).
3. Una vez aprobado por SGC, el flujo se bifurca según el tipo de hallazgo:
   - **NC Menor → 5 porqués**: el responsable de cierre captura PorQué 1 a 3 (obligatorios), 4 y 5 (opcionales) y una lista de acciones (acción, responsable, fecha). Al guardar, queda "Pendiente" y se notifica a QMS. SGC lo aprueba o rechaza (con comentario) desde `/interfaces/sgc/cinco-porques`; si se rechaza, el responsable puede corregir y volver a enviar. Solo cuando queda **"Aprobado"** se habilita subir la evidencia de solución.
   - **NC Mayor (u otro tipo) → acción correctiva**: el responsable de cierre adjunta el archivo de acción correctiva (máx. 20 MB). Queda "Pendiente" y se notifica a QMS. SGC lo aprueba o rechaza (con comentario) desde `/interfaces/sgc/acciones-correctivas`. Solo cuando queda **"Aprobado"** se habilita subir la evidencia de solución.
4. **Evidencia de solución**: una vez habilitada, el responsable de cierre sube los archivos de evidencia (máx. 20 MB cada uno). La primera carga cambia el estatus del hallazgo a **"En Proceso"**. Se notifica a QMS y a los auditores del proceso en cada carga.
5. **Verificación final de QMS** (`/interfaces/qms/verificacion-hallazgos`): QMS revisa la evidencia (puede adjuntar evidencia adicional propia) y decide:
   - **Cerrar hallazgo** → estatus **"Cerrado"**, queda marcado como verificado por QMS. Se notifica al responsable de cierre y a los auditores del proceso.
   - **Rechazar** (requiere comentario) → el hallazgo vuelve a "En Proceso" para que el responsable de cierre suba nueva evidencia.
6. Un hallazgo **"Cerrado"** ya no permite más acciones sobre él (solo consulta).
7. En cualquier momento, si quien consulta el hallazgo es de SGC, puede capturar el **"Seguimiento para el informe"**: tres campos de texto libre (Entrega de análisis a 20 días, Implementación y cierre a 45 días, Verificación) que alimentan directamente el Informe de auditoría.

### 4.4 Notificaciones por correo

Todos los correos usan el mismo cuerpo genérico ("Tiene una notificación pendiente, ingrese a Mis cosas") y se envían siempre con copia oculta fija a `zaid.garcia@meax.mx`. Un fallo al enviar un correo nunca detiene la acción que lo originó.

| Evento | Se notifica a |
|---|---|
| Se crea un hallazgo desde el checklist | QMS |
| Se reenvía a validación un hallazgo corregido | QMS |
| SGC aprueba o rechaza un hallazgo | Auditores del proceso; si se aprueba, también el responsable de cierre |
| Se sube el archivo de acción correctiva | QMS y auditores del proceso |
| SGC aprueba o rechaza la acción correctiva | Responsable de cierre y auditores del proceso |
| Se captura el análisis de 5 porqués | QMS y auditores del proceso |
| SGC aprueba o rechaza los 5 porqués | Responsable de cierre y auditores del proceso |
| Se sube evidencia de solución | QMS y auditores del proceso |
| QMS cierra o rechaza el hallazgo | Responsable de cierre y auditores del proceso |

### 4.5 Informe de auditoría

- **En pantalla** (`/auditorias/{id}/informe`): documento con encabezado y logo de la empresa, datos generales de la auditoría (número, tipo, fechas, auditor líder), Alcance/Objetivo/Criterios/Método, auditores participantes, resumen de resultados (conteo de NC Mayor / NC Menor / Observación de Mejora), tabla detallada de hallazgos (proceso, descripción, requisito, evidencia, tipo, justificación, responsable, estatus, y los tres campos de seguimiento a 20/45 días y verificación), conclusiones de la auditoría y bloque de firma del auditor líder.
- **Descargar Excel**: genera un archivo `.xlsx` con el mismo contenido del informe en pantalla.
- **Imprimir / Descargar PDF**: usa la función de impresión del navegador sobre el mismo documento (los botones de acción no se imprimen); desde el diálogo de impresión se puede elegir "Guardar como PDF".

### 4.6 Importar checklist desde Excel

1. Ir a **"Importar checklist desde Excel"**, elegir el proceso destino y seleccionar un archivo `.xlsx` con el formato "MX-6100-F6".
2. El sistema detecta automáticamente la hoja "Checklist", localiza el encabezado de la tabla y lee cada pregunta con su requisito, deteniéndose al llegar a las firmas de aprobación del formato.
3. Se muestra una vista previa con la cláusula IATF detectada automáticamente cuando es posible identificarla contra el catálogo; si no, se marca "Sin coincidencia".
4. Si el proceso ya tenía preguntas activas, se advierte cuántas serán reemplazadas.
5. Al **"Confirmar importación"**, las preguntas activas anteriores se desactivan (no se eliminan) y se dan de alta las nuevas preguntas importadas.

### 4.7 Otras pantallas de apoyo

- **Mis cosas**: agrupa lo que cada usuario tiene pendiente como auditor (procesos sin programar, checklists por llenar) y como responsable de hallazgos (5 porqués por capturar, hallazgos pendientes de validación SGC), con filtros de vencidos y de esta semana.
- **Auditores**: alta y edición de personas que pueden ser auditor, observador o estar en entrenamiento; se puede buscar en el directorio de Active Directory y usar sus datos para prellenar el formulario.
- **Procesos**: alta, edición, activar/desactivar y eliminar procesos del catálogo (la eliminación se bloquea si el proceso ya tiene auditorías, checklist o unidades de negocio asociadas; en ese caso se recomienda desactivarlo).
- **Preguntas de checklist**: administración del banco de preguntas por proceso, independiente de cualquier auditoría en curso.

---

## 5. Glosario de estatus

### Estatus de la auditoría (`Auditoria.Estatus`)
- **Planeada** — valor inicial al crear la auditoría.
- **En Proceso** — se selecciona manualmente cuando la auditoría está en ejecución.
- **Cerrada** — se selecciona manualmente al finalizar.

### Estatus operativo del hallazgo (`Hallazgo.Estatus`)
- **Pendiente SGC** — recién creado, o el hallazgo fue rechazado/reenviado y espera validación.
- **Abierto** — SGC ya aprobó el hallazgo.
- **En Proceso** — se subió la primera evidencia de solución, o QMS rechazó la evidencia y se espera una nueva.
- **Cerrado** — QMS verificó y cerró el hallazgo.

### Validación SGC del hallazgo (`Hallazgo.EstatusValidacionSgc`)
- **Pendiente** — esperando revisión de SGC.
- **Aprobado** — SGC validó el hallazgo; se habilita el análisis de causa.
- **Rechazado** — SGC lo regresó con comentario; solo el auditor principal puede corregirlo y reenviarlo.

### Acción correctiva — NC Mayor (`AccionCorrectiva.EstatusValidacion`)
- **Pendiente** — archivo recién adjuntado, en espera de SGC.
- **Aprobado** — SGC lo validó; se habilita subir evidencia de solución.
- **Rechazado** — SGC lo regresó con comentario.

### 5 porqués — NC Menor (`HallazgoCincoPorQue.Estatus`)
- **Pendiente** — análisis recién capturado o corregido, en espera de SGC.
- **Aprobado** — SGC lo validó; el formulario queda bloqueado y se habilita subir evidencia de solución.
- **Rechazado** — SGC lo regresó con comentario; el responsable de cierre puede corregir y reenviar.

### Resultado del checklist (`AuditoriaChecklist.Cumple`)
- **Pendiente** — pregunta aún sin responder.
- **Cumple** — conforme.
- **No Cumple** — no conforme; habilita crear un hallazgo si aún no existe uno para esa pregunta.
- **No Aplica** — la pregunta no aplica para ese proceso/auditoría.

---

## 6. Notas importantes de operación

- **Multiusuario**: varias personas pueden trabajar al mismo tiempo en la misma auditoría o el mismo checklist; el sistema guarda por separado. Si la conexión de red se interrumpe brevemente, la sesión se conserva por 10 minutos antes de perderse — aun así, se recomienda guardar frecuentemente.
- **Evidencia obligatoria**: el campo Evidencia es obligatorio tanto al crear un hallazgo como al editarlo; no se puede guardar sin capturarlo.
- **Archivos adjuntos**: el tamaño máximo por archivo (acción correctiva, evidencia de solución, evidencia adicional de QMS) es de 20 MB.
- **Eliminar una auditoría** es una acción exclusiva de SGC/Administrador y borra en cascada sus procesos, checklist y hallazgos asociados — se recomienda usarla con precaución.
