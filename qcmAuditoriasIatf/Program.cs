using Microsoft.AspNetCore.Components.Server;
using Microsoft.EntityFrameworkCore;
using qcmAuditoriasIatf.Components;
using qcmAuditoriasIatf.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using qcmAuditoriasIatf.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.Configure<ActiveDirectoryOptions>(
    builder.Configuration.GetSection("ActiveDirectory"));

builder.Services.AddScoped<ActiveDirectoryService>();

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

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .RequireAuthorization();

app.Run();