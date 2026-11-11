# Alcance: módulo LPA (Layered Process Audits / Auditoría de Proceso por Capas)

Estado: **propuesta para revisión**. Los puntos marcados como *Por confirmar* necesitan una decisión del equipo antes de construir.

## 1. Objetivo

Dar a Calidad una forma de programar, ejecutar y dar seguimiento a auditorías de proceso cortas y frecuentes en piso, hechas por varias capas de la organización (supervisión, gerencia, dirección), con respuesta inmediata a las desviaciones y con indicadores de cumplimiento. Debe poder usarse desde celular o tableta, en unos 10 minutos por auditoría.

## 2. Por qué un módulo aparte

El sistema actual está pensado para auditorías internas IATF: pocas, planeadas a mano, con un flujo de hallazgos pesado (validación SGC, 5 porqués, acción correctiva, verificación QMS). Un LPA es lo opuesto: muchas, recurrentes, de respuesta simple y con una acción de contención rápida. Forzarlo dentro de "Auditorías" lo haría lento. Se propone un módulo propio que **reutilice** lo ya construido.

## 3. Alcance

### Dentro del alcance
- **Plantillas de LPA** por proceso / línea / área, con preguntas de respuesta simple: Cumple, No cumple, No aplica. Cada pregunta puede tener una nota de "cómo verificar".
- **Capas configurables**: nombre, frecuencia (diaria, semanal, mensual), quién audita y qué preguntas aplican a cada capa.
- **Programa de auditorías**: asignación de capa + área/línea + auditor, y generación automática de las tareas pendientes según la frecuencia, con fecha límite.
- **Ejecución en celular/tableta**: lista de preguntas, respuesta con un toque, comentario y foto cuando no cumple, hora de inicio y fin.
- **Acciones por desviación**: cada "No cumple" genera una acción con contención inmediata, responsable, fecha y verificación de cierre.
- **Escalamiento**: avisos por correo por tareas no realizadas y por acciones vencidas, hacia la capa superior.
- **Tablero de indicadores** (ver sección 6).
- Integración con **Inicio** y **Mis cosas** (tareas LPA del día) y con el **menú lateral**.

### Fuera del alcance (primera versión)
- Firma electrónica, integración con MES/ERP, aplicación nativa móvil, lectura de códigos QR por estación y modo sin conexión.
- Cambiar el flujo de hallazgos IATF existente.

## 4. Usuarios y permisos

| Rol | Qué puede hacer |
|---|---|
| SGC / Administrador | Administra plantillas, capas, programa y catálogos; ve todo el tablero. |
| Auditor LPA (cualquier capa) | Ve y ejecuta sus tareas asignadas; da seguimiento a las acciones que le tocan. |
| Responsable de acción | Registra la contención y la acción correctiva de sus desviaciones. |
| Gerencia / Dirección | Consulta el tablero y recibe los escalamientos. |

*Por confirmar:* si todos los auditores LPA deben tener la competencia "CQI-8 LPA" en la matriz, o si cualquier supervisor/gerente puede auditar.

## 5. Flujo

1. SGC define plantillas, capas y programa.
2. El sistema genera cada día/semana/mes las tareas LPA de cada auditor, con fecha límite, y las muestra en **Mis cosas** y por correo.
3. El auditor abre la tarea en el piso, responde las preguntas y finaliza. Si hay "No cumple", captura comentario y foto.
4. Cada "No cumple" crea una **acción**: contención inmediata + responsable + fecha compromiso.
5. El responsable la atiende y el auditor (o Calidad) verifica y cierra.
6. Si una tarea no se hace, o una acción vence, se escala por correo. Si el mismo punto falla de forma repetida, se pide un análisis de causa (5 porqués).
7. El tablero consolida cumplimiento y tendencias.

## 6. Indicadores del tablero

- Cumplimiento del programa: auditorías realizadas contra programadas, por capa, área y auditor.
- % de conformidad por pregunta, por área y por capa.
- Pareto de las preguntas con más "No cumple".
- Acciones abiertas, vencidas y antigüedad promedio; tiempo promedio de cierre.
- Tendencia semanal y mensual; puntos con falla recurrente.
- Exportar a Excel.

## 7. Qué se reutiliza y qué es nuevo

**Se reutiliza:** catálogo de procesos, unidades y líneas; catálogo de auditores y la matriz de competencias; usuarios de MEAX One; servicio de correo; carga de evidencias (fotos); tablas con filtros y paginación; tarjetas y seguimiento por etapas; menú, Inicio y Mis cosas; perfiles SGC/Administrador.

**Es nuevo (tablas):** plantilla y preguntas LPA, capas, programa/asignaciones, auditoría LPA (cada ejecución), respuestas y acciones LPA.

*Decisión de diseño recomendada:* usar una entidad de **acción LPA ligera** y no la entidad `Hallazgo` actual, porque esa está atada a una auditoría IATF y a un proceso asignado, y arrastra el flujo de validación SGC. Si se quiere que las desviaciones LPA se vean junto a los hallazgos IATF en la bandeja global, hay que tomar esa decisión antes de empezar.

## 8. Fases

| Fase | Contenido | Tamaño |
|---|---|---|
| 1. Base | Catálogos (capas, plantillas, preguntas), programa y generación de tareas, ejecución en celular, resultado de cada auditoría, menú y Mis cosas. | Mediano |
| 2. Acciones y escalamiento | Acción por "No cumple", seguimiento y cierre, vencidos, correos, escalamiento, falla recurrente con 5 porqués. | Mediano |
| 3. Tablero | Indicadores de la sección 6, filtros y exportación a Excel. | Pequeño a mediano |
| 4. Mejoras (opcional) | Rotación automática de áreas por auditor, recordatorios, QR por estación, firma. | A definir |

Cada fase se puede probar y usar por separado. Se recomienda arrancar con **un área piloto** antes de abrirlo a toda la planta.

## 9. Riesgos

- **Adopción en piso:** si captura toma más de ~10 minutos o requiere muchos toques, no se usará. Por eso la pantalla de ejecución se diseña primero para celular.
- **Cobertura de usuarios:** los líderes de grupo u operadores que no tengan cuenta en MEAX One no podrían auditar.
- **Conectividad:** sin modo sin conexión, hace falta señal Wi-Fi en el piso.
- **Volumen:** muchas auditorías por día generan muchos datos; se necesita definir cuánto histórico se conserva.

## 10. Preguntas por confirmar

1. ¿Qué áreas, líneas o procesos entran en el piloto y cuáles después?
2. ¿Cuáles son las capas reales, con qué frecuencia y quién audita en cada una (puestos o personas)?
3. ¿Las preguntas son las mismas para todas las capas o cambian por capa?
4. ¿Hay turnos? ¿La frecuencia diaria es por turno?
5. ¿Hay un formato LPA vigente (Excel/papel) que debamos replicar? Si sí, compartirlo.
6. ¿Qué meta de cumplimiento se espera (por ejemplo, 90 %)?
7. ¿Quién verifica y cierra las acciones: el mismo auditor, el líder del área o Calidad?
8. ¿Las desviaciones LPA deben verse junto con los hallazgos IATF o es información separada?
9. ¿Se requiere la competencia "CQI-8 LPA" para auditar?
10. ¿Hay conectividad suficiente en piso y qué dispositivos se usarán (celular personal, tableta de la planta)?

## 11. Criterios de aceptación (resumen)

- Una auditoría LPA se completa desde un celular en menos de 10 minutos con preguntas de 1 toque.
- Las tareas se generan solas según la frecuencia de cada capa y aparecen en Mis cosas.
- Cada "No cumple" crea una acción con responsable y fecha, y su vencimiento dispara un aviso.
- El tablero muestra el cumplimiento del programa por capa y área, y el Pareto de fallas.
