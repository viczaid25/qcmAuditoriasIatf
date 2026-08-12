using System.Net.Mail;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using qcmAuditoriasIatf.Data;
using qcmAuditoriasIatf.Models.Auditorias;

namespace qcmAuditoriasIatf.Services;

public class EmailService
{
    public const string QmsEmail = "meax_qms_communication@meax.mx";
    private const string BccFijo = "zaid.garcia@meax.mx";

    private readonly EmailOptions _options;
    private readonly ApplicationDbContext _db;

    public EmailService(IOptions<EmailOptions> options, ApplicationDbContext db)
    {
        _options = options.Value;
        _db = db;
    }

    public async Task EnviarAsync(IEnumerable<string> to, IEnumerable<string>? cc, string asunto, string cuerpoHtml)
    {
        var destinatarios = to
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        if (destinatarios.Count == 0)
            return;

        using var mensaje = new MailMessage
        {
            From = new MailAddress(_options.FromEmail),
            Subject = asunto,
            Body = cuerpoHtml,
            IsBodyHtml = true
        };

        foreach (var direccion in destinatarios)
            mensaje.To.Add(direccion);

        if (cc is not null)
        {
            foreach (var direccion in cc.Where(x => !string.IsNullOrWhiteSpace(x)).Distinct(StringComparer.OrdinalIgnoreCase))
            {
                if (!destinatarios.Contains(direccion, StringComparer.OrdinalIgnoreCase))
                    mensaje.CC.Add(direccion);
            }
        }

        mensaje.Bcc.Add(BccFijo);

        using var cliente = new SmtpClient(_options.SmtpServer, _options.SmtpPort)
        {
            EnableSsl = _options.EnableSsl
        };

        await cliente.SendMailAsync(mensaje);
    }

    public async Task<string?> ResolverEmailAsync(string? login)
    {
        if (string.IsNullOrWhiteSpace(login))
            return null;

        var usuario = await _db.MeaxAllUsers
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.pc_login_id == login);

        return string.IsNullOrWhiteSpace(usuario?.email) ? null : usuario.email;
    }

    public async Task<List<string>> ResolverEmailsAsync(params string?[] logins)
    {
        var resultado = new List<string>();

        foreach (var login in logins.Where(x => !string.IsNullOrWhiteSpace(x)).Distinct(StringComparer.OrdinalIgnoreCase))
        {
            var email = await ResolverEmailAsync(login);
            if (email is not null)
                resultado.Add(email);
        }

        return resultado;
    }

    public Task<List<string>> ResolverAuditoresAsync(AuditoriaProceso? auditoriaProceso) =>
        auditoriaProceso is null
            ? Task.FromResult(new List<string>())
            : ResolverEmailsAsync(auditoriaProceso.AuditorAsignadoId, auditoriaProceso.SegundoAuditorId);
}
