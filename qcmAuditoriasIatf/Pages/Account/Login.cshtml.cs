using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using qcmAuditoriasIatf.Services;

namespace qcmAuditoriasIatf.Pages.Account;

[AllowAnonymous]
public class LoginModel : PageModel
{
    private readonly ActiveDirectoryService _ad;

    public LoginModel(ActiveDirectoryService ad)
    {
        _ad = ad;
    }

    [BindProperty]
    public InputModel Input { get; set; } = new();

    [BindProperty(SupportsGet = true)]
    public string? ReturnUrl { get; set; }

    public class InputModel
    {
        [Required(ErrorMessage = "El usuario es obligatorio.")]
        [Display(Name = "Usuario")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; } = string.Empty;
    }

    public void OnGet(string? returnUrl = null)
    {
        ReturnUrl = string.IsNullOrWhiteSpace(returnUrl) ? "/b" : returnUrl;
    }

    public async Task<IActionResult> OnPostAsync()
    {
        ReturnUrl = string.IsNullOrWhiteSpace(ReturnUrl) ? "/b" : ReturnUrl;

        if (!ModelState.IsValid)
            return Page();

        var adUser = _ad.Authenticate(Input.Username, Input.Password);

        if (adUser is null)
        {
            ModelState.AddModelError(string.Empty, "Usuario o contraseña inválidos.");
            return Page();
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, adUser.LoginId),
            new(ClaimTypes.Name, string.IsNullOrWhiteSpace(adUser.DisplayName) ? adUser.LoginId : adUser.DisplayName),
            new("pc_login_id", adUser.LoginId)
        };

        if (!string.IsNullOrWhiteSpace(adUser.Email))
            claims.Add(new Claim(ClaimTypes.Email, adUser.Email));

        if (!string.IsNullOrWhiteSpace(adUser.Department))
            claims.Add(new Claim("department", adUser.Department));

        var identity = new ClaimsIdentity(
            claims,
            CookieAuthenticationDefaults.AuthenticationScheme);

        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            principal,
            new AuthenticationProperties
            {
                IsPersistent = true,
                AllowRefresh = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8)
            });

        return LocalRedirect(ReturnUrl!);
    }
}