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
}