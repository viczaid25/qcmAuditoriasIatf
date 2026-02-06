using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace qcmAuditoriasIatf.Models.Evidencias;

[Table("qmcAudEvidencia")]
public class Evidencia
{
    [Key]
    public int EvidenciaId { get; set; }

    [Required]
    public string ReferenciaTipo { get; set; } = string.Empty; // Checklist/Hallazgo/Accion

    public int ReferenciaId { get; set; }

    [Required]
    public string RutaArchivo { get; set; } = string.Empty;

    [Required]
    public string TipoArchivo { get; set; } = string.Empty;

    public DateTime FechaCarga { get; set; } = DateTime.Now;

    [Required]
    public string UsuarioId { get; set; } = "sistema";
}