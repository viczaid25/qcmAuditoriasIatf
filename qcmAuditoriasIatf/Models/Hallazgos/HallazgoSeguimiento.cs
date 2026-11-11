using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace qcmAuditoriasIatf.Models.Hallazgos;

[Table("qmcAudHallazgoSeguimiento")]
public class HallazgoSeguimiento
{
    // Las ediciones que SGC hace a un hallazgo ya aprobado se guardan como seguimiento cuyo
    // comentario empieza con este prefijo y lista un cambio por línea ("Campo: antes → después").
    public const string PrefijoEdicionSgc = "Edición SGC:";

    [Key]
    public int HallazgoSeguimientoId { get; set; }

    public int HallazgoId { get; set; }
    public Hallazgo? Hallazgo { get; set; }

    [Required]
    public DateTime FechaRegistro { get; set; } = DateTime.Now;

    [Required, MaxLength(200)]
    public string UsuarioId { get; set; } = "sistema";

    [Required, MaxLength(2000)]
    public string Comentario { get; set; } = "";

    // Opcional: para dejar evidencia o nota adicional
    [MaxLength(2000)]
    public string? Evidencia { get; set; }

    // Opcional: si en este seguimiento cambiaste el estatus
    [MaxLength(50)]
    public string? EstatusNuevo { get; set; }
}
