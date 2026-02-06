using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace qcmAuditoriasIatf.Models.Evidencias;

[Table("qmcAudHistorialCambio")]
public class HistorialCambio
{
    [Key]
    public int HistorialCambioId { get; set; }

    [Required]
    public string Entidad { get; set; } = string.Empty;

    public int EntidadId { get; set; }

    [Required]
    public string Campo { get; set; } = string.Empty;

    public string? ValorAnterior { get; set; }
    public string? ValorNuevo { get; set; }

    [Required]
    public string UsuarioId { get; set; } = "sistema";

    public DateTime FechaCambio { get; set; } = DateTime.Now;
}