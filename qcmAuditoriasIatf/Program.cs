using Microsoft.AspNetCore.Components.Server;
using Microsoft.EntityFrameworkCore;
using qcmAuditoriasIatf.Components;
using qcmAuditoriasIatf.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));



builder.Services.Configure<CircuitOptions>(o =>
{
    o.DetailedErrors = true;
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
app.UseAntiforgery();

app.MapRazorComponents<App>()
   .AddInteractiveServerRenderMode();

app.Run();