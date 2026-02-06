using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using qcmAuditoriasIatf.Models.Catalogos;

namespace qcmAuditoriasIatf.Models.Auditorias;

[Table("qmcAudAuditoriaChecklist")]
public class AuditoriaChecklist
{
    [Key]
    public int AuditoriaChecklistId { get; set; }

    public int AuditoriaProcesoId { get; set; }
    public AuditoriaProceso? AuditoriaProceso { get; set; }

    public int PreguntaId { get; set; }
    public ChecklistPregunta? Pregunta { get; set; }

    [Required]
    public string Cumple { get; set; } = "Pendiente";

    public string? Evidencia { get; set; }
    public string? Comentarios { get; set; }

    [Required]
    public string AuditorId { get; set; } = "sistema";

    public DateTime FechaRegistro { get; set; } = DateTime.Now;
}