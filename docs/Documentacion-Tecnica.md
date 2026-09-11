# Documentación Técnica del Sistema de Auditorías IATF

**Sistema:** qcmAuditoriasIatf
**Stack:** Blazor Server (.NET 8), Entity Framework Core 8, SQL Server
**Repositorio:** `qcmAuditoriasIatf/qcmAuditoriasIatf/`

---

## 1. Arquitectura general

- **Tipo de aplicación**: Blazor Server (render interactivo del lado del servidor vía SignalR), no Blazor WebAssembly. Configurado con `AddRazorComponents().AddInteractiveServerComponents()`.
- **Persistencia**: Entity Framework Core 8.0.23 sobre SQL Server, base de datos `meax_db` en el servidor `10.228.25.13`.
- **Autenticación**: cookie de autenticación propia, validada contra Active Directory (no ASP.NET Identity, no hay tabla de usuarios/contraseñas local).
- **Paquetes NuGet principales** (`qcmAuditoriasIatf.csproj`, `TargetFramework=net8.0`, `Nullable=enable`):
  - `ClosedXML` 0.105.0 — lectura/escritura de Excel (importación de checklist, exportación del informe).
  - `Microsoft.EntityFrameworkCore` / `.SqlServer` / `.Tools` 8.0.23.
  - `System.DirectoryServices` y `System.DirectoryServices.AccountManagement` 10.0.5 — autenticación contra AD.

### 1.1 Ruta base de la aplicación

Toda la app se sirve bajo el sub-path **`/b`**: en `Program.cs` un middleware redirige (302) cualquier request que no empiece con `/b` a `"/b" + ruta"`, y luego se aplica `app.UsePathBase("/b")`. `App.razor` define `<base href="/b/" />` a juego. Esto es útil para exponer la app detrás de un reverse proxy/IIS que comparte host con otras aplicaciones. Cualquier ruta documentada en este archivo (p. ej. `/auditorias`) debe leerse como `/b/auditorias` en el navegador.

### 1.2 Orden de configuración en `Program.cs`

1. `AddRazorComponents().AddInteractiveServerComponents()`.
2. `AddDbContext<ApplicationDbContext>(UseSqlServer(ConnectionStrings:DefaultConnection))`.
3. `Configure<ActiveDirectoryOptions>` (sección `ActiveDirectory`) + `AddScoped<ActiveDirectoryService>()`.
4. `Configure<EmailOptions>` (sección `EmailSettings`) + `AddScoped<EmailService>()`.
5. `AddRazorPages()` — usado solo por `/Account/Login` y `/Account/Logout`.
6. `AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(...)`:
   - `LoginPath` / `AccessDeniedPath` = `/Account/Login`.
   - Cookie `qcmAuditoriasIatf.Auth`, `Cookie.Path = "/b"`.
   - `ExpireTimeSpan = 8h`, `SlidingExpiration = true`.
7. `AddAuthorization()`, `AddCascadingAuthenticationState()`.
8. `Configure<CircuitOptions>`: `DetailedErrors = true`, `DisconnectedCircuitRetentionPeriod = 10 min` (ajustado desde el default de 3 min para tolerar cortes breves de red sin perder la sesión de Blazor Server).
9. `AddSignalR`: `KeepAliveInterval = 10s` (default 15s), `ClientTimeoutInterval = 60s` (default 30s) — ajustado para redes/proxys corporativos que cierran conexiones inactivas.
10. Redirección + `UsePathBase("/b")` (sección 1.1).
11. **Seed de datos** síncrono al arrancar: abre un scope de DI, resuelve `ApplicationDbContext` y llama `await SeedData.SeedAsync(db)` (ver sección 6).
12. `if (!IsDevelopment()) UseExceptionHandler("/Error")`.
13. `UseStaticFiles()`, `UseAuthentication()`, `UseAuthorization()`, `UseAntiforgery()`.
14. `MapRazorPages()`.
15. Endpoints minimal API (ambos con `RequireAuthorization()`):
    - `GET /evidencias/{id:int}` — descarga de un archivo de evidencia (ver sección 8).
    - `GET /informes/auditoria/{auditoriaId:int}/excel` — genera el informe de auditoría en Excel con ClosedXML.
16. `MapRazorComponents<App>().AddInteractiveServerRenderMode().RequireAuthorization()` — **todo componente Blazor requiere autenticación por defecto**, sea o no que tenga `@attribute [Authorize]` propio.
17. `app.Run()`.

### 1.3 `appsettings.json`

- `ConnectionStrings:DefaultConnection` — cadena de conexión a `meax_db` en `10.228.25.13` (contiene usuario/contraseña de SQL en texto plano; considerar mover a un secreto/variable de entorno en un entorno con más controles).
- `ActiveDirectory` — `LdapPath` (`LDAP://ad.meax.mx`), `NetBiosDomain` (`MEAX`).
- `EmailSettings` — `SmtpServer`, `SmtpPort` (25), `FromEmail` (`auditoriasqc@meax.mx`), `EnableSsl` (false).
- `Logging:LogLevel` — Default Information; `Microsoft.AspNetCore.Components.Server.Circuits` y `Microsoft.AspNetCore.SignalR` en Debug; EF Core en Information.
- `AllowedHosts: "*"`.

---

## 2. Modelo de datos (`Models/`)

Todas las entidades usan Data Annotations (`[Key]`, `[Required]`, `[StringLength]`/`[MaxLength]`) y `[Table("...")]`. Los nombres de tabla usan el prefijo heredado `qmcAud*` (excepto la vista externa de usuarios).

### Catálogos/

| Clase | Tabla | Campos clave |
|---|---|---|
| `TipoHallazgo` | `qmcAudTipoHallazgo` | `TipoHallazgoId` (PK), `Nombre`, `Severidad` |
| `ClausulaIATF` | `qmcAudClausulaIATF` | `ClausulaId` (PK), `Codigo`, `Descripcion`, `Activo` |
| `UnidadNegocio` | `qmcAudUnidadNegocio` | `UnidadNegocioId` (PK), `ProcesoId` (FK), `Codigo`, `Nombre`, `Activo` |
| `Auditor` | `qmcAudAuditor` | `AuditorId` (PK), `MeaxUserId?`, `Nombre`, `Funcion?`, `Tipo` (Auditor/Entrenamiento/Observador), `Departamento?`, `Email?`, `PcLoginId?`, `Activo` |
| `ChecklistPregunta` | `qmcAudChecklistPregunta` | `PreguntaId` (PK), `ClausulaId?` (FK opcional), `ProcesoId` (FK), `Pregunta`, `Requisito?`, `Activo` |
| `Proceso` | `qmcAudProceso` | `ProcesoId` (PK), `Codigo`, `Nombre`, `Descripcion?`, `Area` (default "QMS"), `UnidadNegocio?` (texto libre, distinto de la entidad `UnidadNegocio`), `LineaNombre?`, `Activo` |

### Auditorias/

| Clase | Tabla | Campos clave |
|---|---|---|
| `Auditoria` | `qmcAudAuditoria` | `AuditoriaId` (PK), `FechaInicio`, `FechaFin`, `TipoAuditoria` (default "Interna"), `Objetivo`, `Alcance`, `Criterios`, `Metodos`, `AuditorLiderId`, `Estatus` (default "Planeada"), `Conclusiones?` |
| `AuditoriaProceso` | `qmcAudAuditoriaProceso` | `AuditoriaProcesoId` (PK), `AuditoriaId`/`ProcesoId` (FK), `AuditorAsignadoId?`, `SegundoAuditorId?`, `ObservadorId?`, `AuditadoId?` (CSV de logins), `UnidadNegocioId?`, `FechaProgramada?`, `LineaNombre?`, más campos de contexto de planeación (`ReclamosAuditoriasPasadas`, `ResultadosAuditoriasInternasPrevias`, `SeguimientoAccionesCorrectivasIatf`, `ClausulasIatfTop3NcmMayoresPasadas`, `ClausulasIatfTop3NcmMenoresPasadas`) |
| `AuditoriaChecklist` | `qmcAudAuditoriaChecklist` | `AuditoriaChecklistId` (PK), `AuditoriaProcesoId` (FK), `PreguntaId` (FK), `Cumple` (default "Pendiente"), `Evidencia?`, `Comentarios?`, `AuditorId` (default "sistema"), `FechaRegistro` |

### Hallazgos/

| Clase | Tabla | Campos clave |
|---|---|---|
| `Hallazgo` | `qmcAudHallazgo` | `HallazgoId` (PK), `AuditoriaId`, `ProcesoId`, `ClausulaId?`, `TipoHallazgoId`, `Descripcion`, `JustificacionNoConformidad?`, `Evidencia?`, `ResponsableId`, `ResponsableCierreId?`, `FechaCompromiso` (default hoy+7), `Estatus` (default "Abierto"), `AuditoriaProcesoId`, `PreguntaId?`, `CreadoPorId?`, `FechaCreacion`, `EstatusValidacionSgc` (default "Pendiente"), `RevisadoPorSgcId?`, `FechaRevisionSgc?`, `ComentarioRevisionSgc?`, `VerificadoQms` (bool), `VerificadoPorId?`, `FechaVerificacionQms?`, `ComentarioVerificacionQms?`, `SeguimientoEntregaAnalisis?`, `SeguimientoImplementacionCierre?`, `SeguimientoVerificacion?` (nvarchar(2000) cada uno, agregados para el informe MX-6100-F2) |
| `AccionCorrectiva` | `qmcAudAccionCorrectiva` | `AccionCorrectivaId` (PK), `HallazgoId` (FK), `AnalisisCausa?`, `ResponsableId`, `FechaImplementacion` (default hoy), `FechaVerificacion?`, `Eficaz?` (bool?), `Estatus` (default "Abierta"), `EstatusValidacion` (default "Pendiente" — exclusivo NC Mayor), `RevisadoPorSgcId?`, `FechaRevisionSgc?`, `ComentarioSgc?` |
| `HallazgoSeguimiento` | `qmcAudHallazgoSeguimiento` | `HallazgoSeguimientoId` (PK), `HallazgoId` (FK), `FechaRegistro`, `UsuarioId`, `Comentario`, `Evidencia?`, `EstatusNuevo?` |
| `HallazgoCincoPorQue` | `qmcAudHallazgoCincoPorQue` | `HallazgoCincoPorQueId` (PK), `HallazgoId` (FK, 1:1), `UsuarioRegistroId`, `FechaRegistro`, `PorQue1..PorQue5` (2000 c/u, 1-3 requeridos), `Acciones?` (JSON), `Estatus` (Pendiente/Aprobado/Rechazado), `RevisadoPorSgcId?`, `FechaRevisionSgc?`, `ComentarioSgc?` |

### Evidencias/

| Clase | Tabla | Campos clave |
|---|---|---|
| `Evidencia` | `qmcAudEvidencia` | `EvidenciaId` (PK), `ReferenciaTipo` (discriminador polimórfico: `"Hallazgo"`, `"ValidacionAccion"`, `"VerificacionQms"`), `ReferenciaId` (FK polimórfico, sin constraint de BD), `RutaArchivo` (relativa a `App_Data/evidencias`), `NombreOriginal`, `TipoArchivo` (MIME), `FechaCarga`, `UsuarioId` |
| `HistorialCambio` | `qmcAudHistorialCambio` | `HistorialCambioId` (PK), `Entidad`, `EntidadId`, `Campo`, `ValorAnterior?`, `ValorNuevo?`, `UsuarioId` (default "sistema"), `FechaCambio` — tabla de auditoría genérica, también polimórfica por `(Entidad, EntidadId)` |

### External/

| Clase | Origen | Campos |
|---|---|---|
| `MeaxAllUser` | `meax_all_user` (`HasNoKey()`, excluida de migraciones — tabla/vista externa de solo lectura) | `id`, `pc_login_id`, `username?`, `email?`, `department?`, `position?`, `dep_2?`, `auth` (byte) |

### Seguridad/

| Clase | Tabla | Campos clave |
|---|---|---|
| `UsuarioPerfil` | `qmcAudUsuarioPerfil` | `UsuarioPerfilId` (PK), `PcLoginId`, `Perfil` (`"SGC"` / `"Administrador"`), `Activo` |

---

## 3. `ApplicationDbContext` (`Data/ApplicationDbContext.cs`)

DbSets: `Procesos`, `ClausulasIATF`, `ChecklistPreguntas`, `TiposHallazgo`, `UnidadesNegocio`, `Auditores`, `MeaxAllUsers`, `Auditorias`, `AuditoriaProcesos`, `AuditoriaChecklists`, `Hallazgos`, `AccionesCorrectivas`, `HallazgoSeguimientos`, `HallazgosCincoPorQue`, `Evidencias`, `HistorialCambios`, `UsuariosPerfil`.

Configuración Fluent relevante (`OnModelCreating`):

- La gran mayoría de las relaciones FK usan `DeleteBehavior.Restrict` explícito para evitar el error de SQL Server "multiple cascade paths" (p. ej. `ChecklistPregunta→Proceso`, `ChecklistPregunta→Clausula`, `AuditoriaProceso→Auditoria`, `AuditoriaProceso→Proceso`, `AuditoriaProceso→UnidadNegocio`, `AuditoriaChecklist→AuditoriaProceso`, `Hallazgo→Auditoria/Proceso/Clausula/TipoHallazgo/AuditoriaProceso/Pregunta`, `AccionCorrectiva→Hallazgo`, `UnidadNegocio→Proceso`).
- **Excepciones con Cascade**:
  - `HallazgoSeguimiento → Hallazgo` (Cascade) — al borrar un hallazgo se borran sus seguimientos.
  - `HallazgoCincoPorQue → Hallazgo` (relación 1:1, Cascade).
- Índices únicos: `AuditoriaChecklist (AuditoriaProcesoId, PreguntaId)`; `Hallazgo (AuditoriaProcesoId, PreguntaId)` filtrado `WHERE PreguntaId IS NOT NULL`; `HallazgoCincoPorQue (HallazgoId)`; `UsuarioPerfil (PcLoginId, Perfil)`.
- Índices simples: `Auditor.MeaxUserId`, `Auditor.PcLoginId`, `HallazgoSeguimiento (HallazgoId, FechaRegistro)`.
- `MeaxAllUser`: `.HasNoKey()` + `.ToView("meax_all_user", "dbo")` + `SetIsTableExcludedFromMigrations(true)` — nunca se migra, es de solo lectura.

---

## 4. Migraciones (orden cronológico)

Versión de EF Core / tooling: **8.0.23**.

1. `Baseline_Existente`
2. `AddProcesoUnidadLinea`
3. `Add_UnidadesNegocio_y_Linea`
4. `Add_LineaNombre_To_AuditoriaProceso`
5. `Make_AuditorAsignadoId_Nullable`
6. `Make_Checklist_Use_AuditoriaProcesoId`
7. `Hallazgo_Link_Checklist_Fix`
8. `Hallazgo_Seguimiento`
9. `AddFechaProgramadaToAuditoriaProceso`
10. `Add_Auditores_Catalogo`
11. `AddSegundoAuditorYObservadorEnAuditoriaProceso`
12. `AddResponsableCierreToHallazgo`
13. `AddHallazgoCincoPorQueFlow`
14. `AddCamposPlaneacionAuditoriaYContextoProcesos`
15. `AddAuditadoIdToAuditoriaProceso`
16. `AddAccionesToCincoPorQue`
17. `UpdateCincoPorQueAprobacionSgc`
18. `MakeClausulaIdOptional`
19. `IncreaseProcesoCodigoLength`
20. `IncreaseAuditadoIdLength`
21. `EvidenciaArchivosYAccionCorrectivaSimplificada`
22. `AddVerificacionQmsHallazgo`
23. `AddValidacionAccionCorrectiva`
24. `AddInformeAuditoriaCampos` — agrega `Auditoria.Conclusiones` (nvarchar(max)) y `Hallazgo.SeguimientoEntregaAnalisis`/`SeguimientoImplementacionCierre`/`SeguimientoVerificacion` (nvarchar(2000)); es la migración más reciente, aplicada para soportar el Informe de auditoría.

La evolución refleja el orden real de construcción del sistema: base existente → catálogos de unidades/líneas → corrección de vínculo checklist-hallazgo → campos de seguimiento (segundo auditor, observador, responsable de cierre) → flujo de 5 porqués y su aprobación SGC → campos de planeación/contexto → reestructuración de archivos de evidencia → verificación QMS → validación de acción correctiva → campos del informe final.

> **Nota operativa**: el proyecto **no** aplica migraciones automáticamente al arrancar (no hay `Database.Migrate()` en `Program.cs`); deben aplicarse manualmente con `dotnet ef database update` contra la base de datos real (`meax_db`).

---

## 5. Servicios (`Services/`)

| Servicio | Ciclo de vida | Responsabilidad |
|---|---|---|
| `ActiveDirectoryService` | Scoped | `Authenticate(usuario, contraseña)`: normaliza el login (quita prefijo `DOMINIO\` o sufijo `@dominio`), hace bind contra AD vía `DirectoryEntry` (`NetBiosDomain\sAMAccountName`, `AuthenticationTypes.Secure`) forzando el bind al tocar `.NativeObject`, y si es exitoso ejecuta un `DirectorySearcher` para traer `sAMAccountName`, `displayName`, `mail`, `department`. El login es exitoso **si y solo si** el bind LDAP no lanza excepción — no hay contraseñas propias del sistema. Devuelve `null` en cualquier error (registrado en log) o si no encuentra al usuario. |
| `EmailService` | Scoped | `NotificarAsync(to, cc?)` construye un correo HTML fijo enlazando a `/mis-cosas` y lo envía vía `SmtpClient`/`MailMessage` (puerto 25, sin TLS). Constantes: `QmsEmail = "meax_qms_communication@meax.mx"`, `BccFijo = "zaid.garcia@meax.mx"` (toda notificación lleva esta copia oculta fija). Métodos auxiliares: `ResolverEmailAsync(login)`, `ResolverEmailsAsync(params logins)` (consultan `MeaxAllUsers` por `pc_login_id`), `ResolverAuditoresAsync(auditoriaProceso)` (resuelve auditor asignado + segundo auditor). Todas las llamadas desde las páginas están envueltas en try/catch para que un fallo de correo nunca bloquee la operación de negocio. |
| `ChecklistExcelParser` (estático) | N/A | `Parse(Stream)` con ClosedXML: localiza la hoja "Checklist" (o la primera), detecta el proceso auditado cerca de la etiqueta "Proceso a auditar" (columna G), ubica el encabezado buscando `"#"` en la columna B, y lee `Numero`/`Requisito`/`Pregunta` (columnas B/C/J) hasta encontrar un marcador de pie de página ("elaborado por", "prepared by", "firma de aprobacion", "signature of approval", sin distinguir acentos). El archivo subido nunca se persiste en disco — solo las filas extraídas llegan a la base de datos. |

Modelos de opciones asociados: `ActiveDirectoryOptions` (`LdapPath`, `NetBiosDomain`), `AdUserInfo` (`LoginId`, `DisplayName`, `Email?`, `Department?`), `EmailOptions` (`SmtpServer`, `SmtpPort` default 25, `FromEmail`, `EnableSsl`).

---

## 6. Seed de datos (`Data/SeedData.cs`)

Se ejecuta una sola vez al levantar la aplicación, dentro de un scope de DI creado en `Program.cs`, **antes** de que corra el pipeline de autenticación. Guarda de idempotencia:

```csharp
if (await db.Procesos.AnyAsync())
    return;
```

Es decir: en cada reinicio de la app, si ya existe al menos un `Proceso`, el seeder no hace nada. Solo puebla el catálogo cuando la base está genuinamente vacía. Cuando corre, hace upsert (por `Codigo`) de 16 procesos QMS predefinidos (`MX-0100-D1` Alta Dirección … `MX-6400-D1` Control de Equipos, todos con `Area = "QMS"`, `Activo = true`), y específicamente para el proceso "Producción / Calidad Proceso" (`MX-7000-D1 / MX-6000-D1`) siembra 6 `UnidadNegocio` (Alternador, Marchas, Car Mechatronics, EPS, Multimedia, Car Electronics), también con upsert por `(ProcesoId, Codigo)`.

---

## 7. Autenticación y autorización

### 7.1 Flujo de login

`Pages/Account/Login.cshtml.cs` (Razor Page clásica, `[AllowAnonymous]`, no es un componente Blazor):

1. Recibe `Username`/`Password` por POST.
2. Llama `ActiveDirectoryService.Authenticate(...)` (bind real contra AD, sin almacén de contraseñas local).
3. Si es exitoso, construye un `ClaimsIdentity` con `ClaimTypes.NameIdentifier`, `ClaimTypes.Name`, un claim propio `"pc_login_id"`, y opcionalmente `ClaimTypes.Email`/`"department"`.
4. `HttpContext.SignInAsync` con el esquema de cookie, `IsPersistent = true`, expiración de 8 horas.
5. Redirige a `ReturnUrl` (por defecto `/b`).

`Pages/Account/Logout.cshtml.cs` (`[Authorize]`) simplemente cierra sesión y redirige a `/b/Account/Login`.

### 7.2 Autorización de grano grueso

Todo componente Blazor requiere sesión válida por la configuración global `MapRazorComponents<App>().RequireAuthorization()`, independientemente de si el archivo `.razor` tiene o no su propio `@attribute [Authorize]`. **No existe** un esquema de roles/políticas de ASP.NET (`[Authorize(Roles=...)]`); `NavMenu.razor` muestra los mismos enlaces a cualquier usuario autenticado, sin ocultar opciones por perfil.

### 7.3 Autorización de grano fino (dentro de cada página)

Todos los patrones siguientes se resuelven consultando la base de datos directamente desde el propio componente (no hay middleware ni política central), comparando el `pc_login_id` del usuario actual (trim + case-insensitive) contra un campo de la entidad:

1. **`usuarioEsSgc`** (perfil SGC/Administrador) — repetido literalmente en `HallazgoDetalle.razor`, `AuditoriaDetalle.razor`, `InterfacesSgcHallazgos.razor`, `InterfacesSgcAccionesCorrectivas.razor`, `InterfacesSgcCincoPorques.razor`, `InterfacesQmsVerificacion.razor`:
   ```csharp
   usuarioEsSgc = await Db.UsuariosPerfil.AsNoTracking().AnyAsync(x =>
       x.Activo && x.PcLoginId.ToUpper() == usuarioActualId.ToUpper() &&
       (x.Perfil == "SGC" || x.Perfil == "Administrador"));
   ```
   En las cuatro pantallas `Interfaces/*` funciona como **bloqueo total de la página** (`else if (!usuarioEsSgc) { "Acceso restringido" }`). En `AuditoriaDetalle.razor`/`HallazgoDetalle.razor` solo bloquea acciones puntuales (eliminar auditoría, aprobaciones).
2. **Auditor líder** (`AuditoriaDetalle.razor`) — compara `Auditoria.AuditorLiderId` contra el usuario actual; habilita editar las conclusiones.
3. **Auditor principal del proceso** — dos variantes: en `HallazgoDetalle.razor` compara contra `AuditoriaProceso.AuditorAsignadoId`; en `HallazgoEditar.razor` funciona como **bloqueo total de la página** ("Solo el auditor principal del proceso puede editar este hallazgo").
4. **Responsable de cierre** (`HallazgoDetalle.razor`) — compara contra `Hallazgo.ResponsableCierreId`; habilita subir evidencia de cierre/acción correctiva.

No existe una autorización específica basada en "segundo auditor"/"observador" más allá de mostrar esos datos; esos campos son solo informativos/de asignación.

---

## 8. Almacenamiento de archivos

- Los archivos subidos (evidencia, acción correctiva) se guardan bajo `App_Data/evidencias/...`, relativo a `IWebHostEnvironment.ContentRootPath`; la carpeta se crea en tiempo de ejecución (`Directory.CreateDirectory`) y no forma parte del control de versiones.
- Rutas concretas usadas en `HallazgoDetalle.razor`:
  - `App_Data/evidencias/hallazgos/{hallazgoId}/{Guid:N}{ext}` — evidencia de cierre (`ReferenciaTipo = "Hallazgo"`).
  - `App_Data/evidencias/validacion-accion/{hallazgoId}/{Guid:N}{ext}` — archivo de acción correctiva (`ReferenciaTipo = "ValidacionAccion"`).
  - Evidencia adicional de QMS usa `ReferenciaTipo = "VerificacionQms"`.
- Límite de tamaño: 20 MB por archivo. El nombre físico en disco siempre es un GUID nuevo (nunca el nombre original) para evitar colisiones o inyección de rutas vía nombre de archivo; el nombre original solo se conserva en `Evidencia.NombreOriginal` para mostrarlo/descargarlo.
- `Evidencia.ReferenciaTipo`/`ReferenciaId` es un patrón **polimórfico sin FK real** en la base de datos.
- El Excel subido en "Importar checklist" **no** se guarda en disco — se procesa completo en memoria con `ChecklistExcelParser.Parse(Stream)`.

### Endpoint de descarga — `GET /evidencias/{id:int}` (`RequireAuthorization()`)

```csharp
var carpetaBase = Path.Combine(env.ContentRootPath, "App_Data", "evidencias");
var rutaCompleta = Path.GetFullPath(Path.Combine(carpetaBase, evidencia.RutaArchivo));
if (!rutaCompleta.StartsWith(carpetaBase, StringComparison.OrdinalIgnoreCase) || !File.Exists(rutaCompleta))
    return Results.NotFound();
```

Protección contra path traversal: se resuelve la ruta absoluta con `Path.GetFullPath` y se exige que siga empezando dentro de la carpeta base — esto neutraliza cualquier `..\` que pudiera colarse en `RutaArchivo` (en la práctica `RutaArchivo` siempre se construye con un GUID nuevo, por lo que esto es una defensa adicional, no la mitigación principal). El content-type servido viene de `Evidencia.TipoArchivo` (default `application/octet-stream`) y el nombre de descarga es `Evidencia.NombreOriginal`.

> **Nota de seguridad**: este endpoint solo valida `[Authorize]` (cualquier usuario con sesión), **no** valida que quien descarga tenga permiso sobre ese hallazgo/auditoría específico. Cualquier usuario autenticado que conozca o adivine un `id` de evidencia puede descargarla.

### Endpoint de informe — `GET /informes/auditoria/{auditoriaId:int}/excel`

Genera un `XLWorkbook` (ClosedXML) en memoria replicando el contenido del Informe en pantalla (encabezado, metadatos, Alcance/Objetivo/Criterios/Método, auditores participantes, conteos NCM/NCm/OM, tabla detallada de hallazgos, conclusiones) y lo devuelve con `Results.File(..., "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Informe_auditoria_{id}.xlsx")`.

---

## 9. Frontend / estilos ("MEAX One")

- **`wwwroot/css/meaxone-template.css`** (~1350 líneas) — hoja de estilos compartida "MEAX Hub — Design System v1.0": variables CSS (`:root`) para la paleta de marca (tema azul/marino, colores de acento por departamento `--dept-*`), constantes de layout (`--mo-sidebar-w`, `--mo-navbar-h`), tipografía Montserrat, overlay de carga (`#mo-loader`), y los componentes compartidos (sidebar, tablas `.mo-table`, botones, tarjetas `.panel-card`, estados vacíos `.empty-state`). Se carga globalmente en `Components/App.razor` junto con `bootstrap.min.css` y `app.css`. Incluye un bloque `@media print` global que oculta `#mo-navbar`, `#mo-sidebar` y cualquier elemento `.no-print` al imprimir.
- **CSS isolation por componente** (`.razor.css`) — cada página puede tener su propio archivo de estilos, compilados en `qcmAuditoriasIatf.styles.css` (también referenciado en `App.razor`), como complemento del design system compartido.
- **`wwwroot/js/meaxone-table.js`** (~180 líneas) — script sin dependencias que habilita filtrado por columna y paginación en cualquier tabla marcada con `data-mo-table`/`class="mo-table"` y un `.mo-table-filter-row` de inputs/`<select>` con `data-mo-filter-col`; se autoinicializa en `DOMContentLoaded`.
- **`wwwroot/js/calendar-export.js`** (~115 líneas) — usado por `AuditoriaCalendario.razor` junto con `html2canvas`/`jsPDF` (cargados por CDN en `App.razor`) para exportar el calendario como imagen o PDF.
- **`App.razor`** define `<base href="/b/" />`, en consonancia con `UsePathBase("/b")` del lado del servidor.

### Lista completa de archivos `.razor.css`

```
Components/Pages/Home.razor.css
Components/Pages/MisCosas.razor.css
Components/Pages/Auditorias/Auditorias.razor.css
Components/Pages/Auditorias/AuditoriaProcesoAsignar.razor.css
Components/Pages/Auditorias/AuditoriaCalendario.razor.css
Components/Pages/Auditorias/AuditoriaNueva.razor.css
Components/Pages/Auditorias/AuditoriaProcesoChecklist.razor.css
Components/Pages/Auditorias/AuditoriaDetalle.razor.css
Components/Pages/Auditorias/AuditoriaInforme.razor.css
Components/Pages/Auditores/AuditorEditar.razor.css
Components/Pages/Auditores/AuditoresListado.razor.css
Components/Pages/Hallazgos/HallazgoNuevoDesdeChecklist.razor.css
Components/Pages/Hallazgos/HallazgoDetalle.razor.css
Components/Pages/Hallazgos/HallazgoEditar.razor.css
Components/Pages/Hallazgos/HallazgosGlobal.razor.css
Components/Pages/Hallazgos/HallazgosBandeja.razor.css
Components/Pages/Interfaces/InterfacesSgcCincoPorques.razor.css
Components/Pages/Interfaces/InterfacesQmsVerificacion.razor.css
Components/Pages/Interfaces/InterfacesSgcAccionesCorrectivas.razor.css
Components/Pages/Interfaces/InterfacesSgcHallazgos.razor.css
Components/Pages/Procesos/ChecklistPreguntas.razor.css
```

---

## 10. Consideraciones técnicas y puntos de atención

- **Sin migración automática**: hay que ejecutar `dotnet ef database update` manualmente contra `meax_db` tras cada nueva migración; el servidor no lo hace por sí solo al arrancar.
- **Reinicio de proceso requerido**: cualquier cambio de código C#/Razor en el servidor requiere reiniciar el proceso de la aplicación (recargar el navegador no es suficiente en Blazor Server) para que los usuarios lo vean reflejado.
- **`DbContext` por circuito, no por página**: al ser Blazor Server, el `ApplicationDbContext` inyectado vive durante todo el circuito (la sesión SignalR del usuario), no solo durante una página — hay que llamar `Db.ChangeTracker.Clear()` al recargar datos en páginas de larga permanencia para evitar entidades "stale" ya rastreadas de una navegación anterior.
- **Guardado por campos modificados ("dirty fields")**: `AuditoriaProcesoChecklist.razor` compara cada valor contra su valor original cargado y solo persiste los campos que el usuario realmente cambió, evitando que un usuario con una copia desactualizada del checklist sobrescriba respuestas guardadas por otro usuario concurrente.
- **Cadena de conexión con credenciales en texto plano** en `appsettings.json` — evaluar mover a un almacén de secretos si el entorno de despliegue lo permite.
- **Descarga de evidencia sin control de propiedad** (`GET /evidencias/{id}`) — solo exige sesión autenticada, no pertenencia al hallazgo/auditoría; ver sección 8.
- **Páginas de catálogo `Procesos` sin `[Authorize]` explícito**: aunque quedan protegidas por el requisito global de sesión de `MapRazorComponents(...).RequireAuthorization()`, no tienen ninguna de las verificaciones de perfil (`usuarioEsSgc`, etc.) que sí tienen casi todas las demás páginas — cualquier usuario autenticado puede dar de alta, editar o desactivar procesos.
- **Generación de PDF sin librería de terceros**: el Informe de auditoría usa `window.print()` (impresión nativa del navegador) en vez de una librería de PDF del lado del servidor (QuestPDF, iText7, Syncfusion), decisión tomada deliberadamente para evitar costos de licenciamiento comercial.
