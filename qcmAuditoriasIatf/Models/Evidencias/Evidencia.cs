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
    public string NombreOriginal { get; set; } = string.Empty;

    [Required]
    public string TipoArchivo { get; set; } = string.Empty;

    public DateTime FechaCarga { get; set; } = DateTime.Now;

    // Fecha en que se tomó el curso/certificación; solo aplica a certificados de
    // competencias y de auditor líder (la vigencia de 2 años se calcula a partir de ella).
    [Column(TypeName = "date")]
    public DateTime? FechaCertificado { get; set; }

    [Required]
    public string UsuarioId { get; set; } = "sistema";
}