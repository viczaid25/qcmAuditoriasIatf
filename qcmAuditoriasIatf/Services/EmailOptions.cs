namespace qcmAuditoriasIatf.Services;

public class EmailOptions
{
    public string SmtpServer { get; set; } = string.Empty;
    public int SmtpPort { get; set; } = 25;
    public string FromEmail { get; set; } = string.Empty;
    public bool EnableSsl { get; set; }
}
