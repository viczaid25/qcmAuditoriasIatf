using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace qcmAuditoriasIatf.Models.Hallazgos;

[Table("qmcAudHallazgoCincoPorQue")]
public class HallazgoCincoPorQue
{
    [Key]
    public int HallazgoCincoPorQueId { get; set; }

    public int HallazgoId { get; set; }
    public Hallazgo? Hallazgo { get; set; }

    [Required, MaxLength(100)]
    public string UsuarioRegistroId { get; set; } = "sistema";

    [Required]
    public DateTime FechaRegistro { get; set; } = DateTime.Now;

    [Required, MaxLength(2000)]
    public string PorQue1 { get; set; } = string.Empty;

    [Required, MaxLength(2000)]
    public string PorQue2 { get; set; } = string.Empty;

    [Required, MaxLength(2000)]
    public string PorQue3 { get; set; } = string.Empty;

    [Required, MaxLength(2000)]
    public string PorQue4 { get; set; } = string.Empty;

    [Required, MaxLength(2000)]
    public string PorQue5 { get; set; } = string.Empty;

    [Required, MaxLength(50)]
    public string Estatus { get; set; } = "Pendiente"; // Pendiente / Aprobado / Rechazado

    public bool? AprobacionAuditorAsignado { get; set; }

    [MaxLength(100)]
    public string? AprobadoPorAuditorAsignadoId { get; set; }

    public DateTime? FechaAprobacionAuditorAsignado { get; set; }

    [MaxLength(2000)]
    public string? ComentarioAuditorAsignado { get; set; }

    public bool? AprobacionSegundoAuditor { get; set; }

    [MaxLength(100)]
    public string? AprobadoPorSegundoAuditorId { get; set; }

    public DateTime? FechaAprobacionSegundoAuditor { get; set; }

    [MaxLength(2000)]
    public string? ComentarioSegundoAuditor { get; set; }
}