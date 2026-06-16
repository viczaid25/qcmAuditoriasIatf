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

    [StringLength(100)]
    public string? AuditorAsignadoId { get; set; }

    [StringLength(100)]
    public string? SegundoAuditorId { get; set; }

    [StringLength(100)]
    public string? ObservadorId { get; set; }

    [StringLength(100)]
    public string? AuditadoId { get; set; }

    public int? UnidadNegocioId { get; set; }
    public UnidadNegocio? UnidadNegocio { get; set; }

    [DataType(DataType.Date)]
    public DateTime? FechaProgramada { get; set; }

    public string? LineaNombre { get; set; }

    public string? ReclamosAuditoriasPasadas { get; set; }
    public string? ResultadosAuditoriasInternasPrevias { get; set; }
    public string? SeguimientoAccionesCorrectivasIatf { get; set; }
    public string? ClausulasIatfTop3NcmMayoresPasadas { get; set; }
    public string? ClausulasIatfTop3NcmMenoresPasadas { get; set; }
}