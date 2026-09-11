using Microsoft.AspNetCore.Components.Server;
using Microsoft.EntityFrameworkCore;
using qcmAuditoriasIatf.Components;
using qcmAuditoriasIatf.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using qcmAuditoriasIatf.Services;
using qcmAuditoriasIatf.Models.Hallazgos;
using ClosedXML.Excel;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.Configure<ActiveDirectoryOptions>(
    builder.Configuration.GetSection("ActiveDirectory"));

builder.Services.AddScoped<ActiveDirectoryService>();

builder.Services.Configure<EmailOptions>(
    builder.Configuration.GetSection("EmailSettings"));

builder.Services.AddScoped<EmailService>();

builder.Services.AddRazorPages();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/Login";
        options.Cookie.Name = "qcmAuditoriasIatf.Auth";
        options.Cookie.Path = "/b";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.SlidingExpiration = true;
    });

builder.Services.AddAuthorization();
builder.Services.AddCascadingAuthenticationState();

builder.Services.Configure<CircuitOptions>(o =>
{
    o.DetailedErrors = true;

    // Da más margen para que el circuito (la conexión en tiempo real con el navegador)
    // sobreviva a cortes breves de red sin forzar una recarga completa de la página,
    // que borraría lo que el usuario haya escrito y aún no haya guardado.
    o.DisconnectedCircuitRetentionPeriod = TimeSpan.FromMinutes(10); // antes: 3 minutos (default)
});

builder.Services.AddSignalR(o =>
{
    // Ping más frecuente para que la conexión no parezca "inactiva" ante proxies/firewalls
    // corporativos que cierran conexiones sin tráfico, y más tolerancia antes de darla por caída.
    o.KeepAliveInterval = TimeSpan.FromSeconds(10); // antes: 15 segundos (default)
    o.ClientTimeoutInterval = TimeSpan.FromSeconds(60); // antes: 30 segundos (default)
});

var app = builder.Build();

app.Use(async (ctx, next) =>
{
    if (!ctx.Request.Path.StartsWithSegments("/b"))
    {
        ctx.Response.Redirect("/b" + ctx.Request.Path + ctx.Request.QueryString);
        return;
    }
    await next();
});

app.UsePathBase("/b");

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await SeedData.SeedAsync(db);
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();

app.MapRazorPages();

app.MapGet("/evidencias/{id:int}", async (int id, ApplicationDbContext db, IWebHostEnvironment env) =>
{
    var evidencia = await db.Evidencias
        .AsNoTracking()
        .FirstOrDefaultAsync(e => e.EvidenciaId == id);

    if (evidencia is null)
        return Results.NotFound();

    var carpetaBase = Path.Combine(env.ContentRootPath, "App_Data", "evidencias");
    var rutaCompleta = Path.GetFullPath(Path.Combine(carpetaBase, evidencia.RutaArchivo));

    if (!rutaCompleta.StartsWith(carpetaBase, StringComparison.OrdinalIgnoreCase) || !File.Exists(rutaCompleta))
        return Results.NotFound();

    var tipoContenido = string.IsNullOrWhiteSpace(evidencia.TipoArchivo)
        ? "application/octet-stream"
        : evidencia.TipoArchivo;

    return Results.File(rutaCompleta, tipoContenido, evidencia.NombreOriginal);
}).RequireAuthorization();

app.MapGet("/informes/auditoria/{auditoriaId:int}/excel", async (int auditoriaId, ApplicationDbContext db) =>
{
    var auditoria = await db.Auditorias
        .AsNoTracking()
        .FirstOrDefaultAsync(a => a.AuditoriaId == auditoriaId);

    if (auditoria is null)
        return Results.NotFound();

    var hallazgos = await db.Hallazgos
        .AsNoTracking()
        .Include(h => h.Proceso)
        .Include(h => h.Clausula)
        .Include(h => h.Pregunta)
        .Include(h => h.TipoHallazgo)
        .Where(h => h.AuditoriaId == auditoriaId)
        .OrderBy(h => h.HallazgoId)
        .ToListAsync();

    var procesos = await db.AuditoriaProcesos
        .AsNoTracking()
        .Where(x => x.AuditoriaId == auditoriaId)
        .ToListAsync();

    var loginsAuditores = procesos
        .SelectMany(p => new[] { p.AuditorAsignadoId, p.SegundoAuditorId })
        .Where(x => !string.IsNullOrWhiteSpace(x))
        .Select(x => x!.Trim())
        .Distinct(StringComparer.OrdinalIgnoreCase)
        .ToList();

    var idsPersonas = loginsAuditores
        .Append(auditoria.AuditorLiderId)
        .Concat(hallazgos.SelectMany(h => new string?[] { h.ResponsableId, h.ResponsableCierreId }))
        .Where(x => !string.IsNullOrWhiteSpace(x))
        .Select(x => x!.Trim())
        .Distinct(StringComparer.OrdinalIgnoreCase)
        .ToList();

    var personasDisplay = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

    if (idsPersonas.Count > 0)
    {
        var usuarios = await db.MeaxAllUsers
            .AsNoTracking()
            .Where(x =>
                (!string.IsNullOrWhiteSpace(x.pc_login_id) && idsPersonas.Contains(x.pc_login_id)) ||
                (!string.IsNullOrWhiteSpace(x.username) && idsPersonas.Contains(x.username)))
            .ToListAsync();

        foreach (var u in usuarios)
        {
            var nombre = string.IsNullOrWhiteSpace(u.username) ? u.pc_login_id : u.username!;
            if (!string.IsNullOrWhiteSpace(u.pc_login_id))
                personasDisplay[u.pc_login_id.Trim()] = nombre ?? u.pc_login_id;
        }
    }

    string MostrarPersona(string? valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            return "Sin asignar";

        var clave = valor.Trim();
        return personasDisplay.TryGetValue(clave, out var nombre) ? nombre : clave;
    }

    string TextoRequisito(Hallazgo h)
    {
        if (h.Clausula is not null)
            return $"{h.Clausula.Codigo} - {h.Clausula.Descripcion}";

        return h.Pregunta?.Requisito ?? "";
    }

    string TipoAbreviado(Hallazgo h)
    {
        var nombre = h.TipoHallazgo?.Nombre ?? "";

        if (nombre.Contains("Mayor", StringComparison.OrdinalIgnoreCase)) return "NCM";
        if (nombre.Contains("Menor", StringComparison.OrdinalIgnoreCase)) return "NCm";
        if (nombre.Contains("Mejora", StringComparison.OrdinalIgnoreCase)) return "OM";

        return nombre;
    }

    using var wb = new XLWorkbook();
    var ws = wb.Worksheets.Add("Informe");

    ws.Cell(1, 1).Value = "Mitsubishi Electric Automotive de México, S.A. de C.V.";
    ws.Cell(1, 1).Style.Font.Bold = true;

    ws.Cell(2, 1).Value = "Informe de auditoría interna";
    ws.Cell(2, 1).Style.Font.Bold = true;
    ws.Cell(2, 1).Style.Font.FontSize = 14;

    ws.Cell(4, 1).Value = "Auditoría";
    ws.Cell(4, 2).Value = $"#{auditoria.AuditoriaId}";
    ws.Cell(4, 3).Value = "Tipo";
    ws.Cell(4, 4).Value = auditoria.TipoAuditoria;

    ws.Cell(5, 1).Value = "Fecha";
    ws.Cell(5, 2).Value = $"{auditoria.FechaInicio:yyyy-MM-dd} - {auditoria.FechaFin:yyyy-MM-dd}";
    ws.Cell(5, 3).Value = "Auditor líder";
    ws.Cell(5, 4).Value = MostrarPersona(auditoria.AuditorLiderId);

    ws.Cell(7, 1).Value = "Alcance de auditoría";
    ws.Cell(7, 1).Style.Font.Bold = true;
    ws.Cell(8, 1).Value = auditoria.Alcance;

    ws.Cell(10, 1).Value = "Objetivo de auditoría";
    ws.Cell(10, 1).Style.Font.Bold = true;
    ws.Cell(11, 1).Value = auditoria.Objetivo;

    ws.Cell(13, 1).Value = "Criterios";
    ws.Cell(13, 1).Style.Font.Bold = true;
    ws.Cell(14, 1).Value = auditoria.Criterios;

    ws.Cell(16, 1).Value = "Método";
    ws.Cell(16, 1).Style.Font.Bold = true;
    ws.Cell(17, 1).Value = auditoria.Metodos;

    ws.Cell(19, 1).Value = "Auditores participantes";
    ws.Cell(19, 1).Style.Font.Bold = true;
    ws.Cell(20, 1).Value = loginsAuditores.Count == 0
        ? "Sin registrar."
        : string.Join(", ", loginsAuditores.Select(MostrarPersona).Distinct().OrderBy(x => x));

    var totalNCM = hallazgos.Count(h => string.Equals(h.TipoHallazgo?.Nombre, "NC Mayor", StringComparison.OrdinalIgnoreCase));
    var totalNCm = hallazgos.Count(h => string.Equals(h.TipoHallazgo?.Nombre, "NC Menor", StringComparison.OrdinalIgnoreCase));
    var totalOM = hallazgos.Count(h => string.Equals(h.TipoHallazgo?.Nombre, "Observación de Mejora", StringComparison.OrdinalIgnoreCase));

    ws.Cell(22, 1).Value = "Resultados de auditoría";
    ws.Cell(22, 1).Style.Font.Bold = true;
    ws.Cell(23, 1).Value = "No conformidades mayores (NCM)";
    ws.Cell(23, 2).Value = totalNCM;
    ws.Cell(24, 1).Value = "No conformidades menores (NCm)";
    ws.Cell(24, 2).Value = totalNCm;
    ws.Cell(25, 1).Value = "Observaciones de mejora (OM)";
    ws.Cell(25, 2).Value = totalOM;

    var filaTabla = 27;
    ws.Cell(filaTabla, 1).Value = "Informe detallado de hallazgos";
    ws.Cell(filaTabla, 1).Style.Font.Bold = true;
    filaTabla++;

    var encabezados = new[]
    {
        "No.", "Proceso", "Declaración", "Requisito", "Evidencia", "Tipo", "Justificación",
        "Responsable", "Estatus", "Entrega de análisis (20 días)", "Implementación y cierre (45 días)", "Verificación"
    };

    for (var col = 0; col < encabezados.Length; col++)
    {
        var celda = ws.Cell(filaTabla, col + 1);
        celda.Value = encabezados[col];
        celda.Style.Font.Bold = true;
        celda.Style.Fill.BackgroundColor = XLColor.FromHtml("#F8FAFC");
    }

    var filaDatos = filaTabla + 1;

    foreach (var h in hallazgos)
    {
        ws.Cell(filaDatos, 1).Value = h.HallazgoId;
        ws.Cell(filaDatos, 2).Value = h.Proceso is null ? "" : $"{h.Proceso.Codigo} - {h.Proceso.Nombre}";
        ws.Cell(filaDatos, 3).Value = h.Descripcion;
        ws.Cell(filaDatos, 4).Value = TextoRequisito(h);
        ws.Cell(filaDatos, 5).Value = h.Evidencia ?? "";
        ws.Cell(filaDatos, 6).Value = TipoAbreviado(h);
        ws.Cell(filaDatos, 7).Value = h.JustificacionNoConformidad ?? "";
        ws.Cell(filaDatos, 8).Value = MostrarPersona(h.ResponsableId);
        ws.Cell(filaDatos, 9).Value = h.Estatus;
        ws.Cell(filaDatos, 10).Value = h.SeguimientoEntregaAnalisis ?? "";
        ws.Cell(filaDatos, 11).Value = h.SeguimientoImplementacionCierre ?? "";
        ws.Cell(filaDatos, 12).Value = h.SeguimientoVerificacion ?? "";

        for (var col = 1; col <= encabezados.Length; col++)
            ws.Cell(filaDatos, col).Style.Alignment.WrapText = true;

        filaDatos++;
    }

    var filaConclusiones = filaDatos + 1;
    ws.Cell(filaConclusiones, 1).Value = "Conclusiones de auditoría";
    ws.Cell(filaConclusiones, 1).Style.Font.Bold = true;
    ws.Cell(filaConclusiones + 1, 1).Value = string.IsNullOrWhiteSpace(auditoria.Conclusiones)
        ? "Sin conclusiones capturadas."
        : auditoria.Conclusiones;

    ws.Columns(1, encabezados.Length).AdjustToContents(1, filaDatos - 1);
    ws.Column(3).Width = Math.Min(ws.Column(3).Width, 60);
    ws.Column(4).Width = Math.Min(ws.Column(4).Width, 60);
    ws.Column(7).Width = Math.Min(ws.Column(7).Width, 60);

    using var stream = new MemoryStream();
    wb.SaveAs(stream);
    stream.Position = 0;

    var nombreArchivo = $"Informe_auditoria_{auditoria.AuditoriaId}.xlsx";

    return Results.File(
        stream.ToArray(),
        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
        nombreArchivo);
}).RequireAuthorization();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .RequireAuthorization();

app.Run();