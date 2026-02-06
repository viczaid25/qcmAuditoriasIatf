using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using qcmAuditoriasIatf.Models.Catalogos;

namespace qcmAuditoriasIatf.Models.Auditorias;

[Table("qmcAudAuditoriaProceso")]
public class AuditoriaProceso
{
    [Key]
    public int AuditoriaProcesoId { get; set; }

    public int AuditoriaId { get; set; }
    public Auditoria? Auditoria { get; set; }

    public int ProcesoId { get; set; }
    public Proceso Proceso { get; set; } = default!;

    // por ahora string (cuando metas Identity puede ser el UserId)
    [StringLength(100)]
    public string? AuditorAsignadoId { get; set; }

    public int? UnidadNegocioId { get; set; }
    public UnidadNegocio? UnidadNegocio { get; set; }

    [DataType(DataType.Date)]
    public DateTime? FechaProgramada { get; set; }  // null mientras no se programe


    // solo aplica para Producción (o cuando UnidadNegocioId != null)
    public string? LineaNombre { get; set; }
}