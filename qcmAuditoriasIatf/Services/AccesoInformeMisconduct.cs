using Microsoft.EntityFrameworkCore;
using qcmAuditoriasIatf.Data;

namespace qcmAuditoriasIatf.Services;

// Además de SGC y de quienes SGC designa como revisores, pueden consultar (solo lectura) el
// informe extendido de Misconduct las personas del departamento QC con puesto DGM o MGR
// (el puesto AMGR no cuenta).
public static class AccesoInformeMisconduct
{
    public const string Departamento = "QC";
    public static readonly string[] Puestos = { "DGM", "MGR" };

    public static async Task<bool> EsDirectivoCalidadAsync(ApplicationDbContext db, string? login)
    {
        var normalizado = Normalizar(login);

        if (string.IsNullOrEmpty(normalizado))
            return false;

        var puestos = Puestos;

        return await db.MeaxAllUsers
            .AsNoTracking()
            .AnyAsync(u =>
                u.pc_login_id.ToUpper() == normalizado &&
                u.department != null && u.department.Trim().ToUpper() == Departamento &&
                u.position != null && puestos.Contains(u.position.Trim().ToUpper()));
    }

    private static string Normalizar(string? valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            return string.Empty;

        var v = valor.Trim();

        if (v.Contains('\\'))
            v = v[(v.LastIndexOf('\\') + 1)..];

        if (v.Contains('@'))
            v = v[..v.IndexOf('@')];

        return v.Trim().ToUpperInvariant();
    }
}
