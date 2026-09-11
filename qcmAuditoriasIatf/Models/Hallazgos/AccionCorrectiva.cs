using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace qcmAuditoriasIatf.Models.Hallazgos;

[Table("qmcAudAccionCorrectiva")]
public class AccionCorrectiva
{
    [Key]
    public int AccionCorrectivaId { get; set; }

    public int HallazgoId { get; set; }
    public Hallazgo? Hallazgo { get; set; }

    public string? AnalisisCausa { get; set; }

    [Required]
    public string ResponsableId { get; set; } = "sistema";

    public DateTime FechaImplementacion { get; set; } = DateTime.Today;

    public DateTime? FechaVerificacion { get; set; }
    public bool? Eficaz { get; set; }

    [Required]
    public string Estatus { get; set; } = "Abierta";

    // Validación SGC del archivo de acción correctiva, exclusiva de NC Mayor.
    [Required, StringLength(50)]
    public string EstatusValidacion { get; set; } = "Pendiente";
    // Pendiente / Aprobado / Rechazado

    [StringLength(100)]
    public string? RevisadoPorSgcId { get; set; }

    public DateTime? FechaRevisionSgc { get; set; }

    [StringLength(2000)]
    public string? ComentarioSgc { get; set; }
}