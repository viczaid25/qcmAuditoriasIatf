namespace qcmAuditoriasIatf.Services;

public enum EstadoVigencia
{
    NoAplica,
    SinCertificado,
    SinFecha,
    Vigente,
    PorVencer,
    Vencido
}

public sealed record VigenciaCertificado(
    EstadoVigencia Estado,
    DateTime? FechaToma,
    DateTime? FechaVence,
    int? DiasRestantes);

// Las competencias obligatorias (**) de la matriz MX-6100-F7 se renuevan cada 2 años
// a partir de la fecha en que se tomó el curso; la vigencia se calcula con el certificado
// más reciente. Las no obligatorias no vencen.
public static class VigenciaCertificados
{
    public const int AnosVigencia = 2;
    public const int DiasAviso = 90;

    public static VigenciaCertificado Evaluar(bool obligatoria, IEnumerable<DateTime?> fechasCertificados, DateTime hoy)
    {
        var certificados = fechasCertificados.ToList();
        var fechas = certificados
            .Where(f => f.HasValue)
            .Select(f => f!.Value.Date)
            .ToList();

        DateTime? ultima = fechas.Count > 0 ? fechas.Max() : null;

        if (!obligatoria)
            return new VigenciaCertificado(EstadoVigencia.NoAplica, ultima, null, null);

        if (certificados.Count == 0)
            return new VigenciaCertificado(EstadoVigencia.SinCertificado, null, null, null);

        if (ultima is null)
            return new VigenciaCertificado(EstadoVigencia.SinFecha, null, null, null);

        var vence = ultima.Value.AddYears(AnosVigencia);
        var dias = (vence - hoy.Date).Days;

        var estado = dias < 0
            ? EstadoVigencia.Vencido
            : dias <= DiasAviso
                ? EstadoVigencia.PorVencer
                : EstadoVigencia.Vigente;

        return new VigenciaCertificado(estado, ultima, vence, dias);
    }
}
