using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace qcmAuditoriasIatf.Models.Auditorias;

// Evaluación de desempeño de un auditor dentro de una auditoría (formato MX-6100-F3).
// Una sola evaluación cubre toda la participación de la persona en esa auditoría
// (no una por proceso). Se genera automáticamente en estatus "Pendiente" cuando la
// auditoría se marca como Cerrada, para cada auditor principal/segundo auditor que
// haya participado (los observadores no se evalúan).
[Table("qmcAudEvaluacionAuditor")]
public class EvaluacionAuditor
{
    [Key]
    public int EvaluacionAuditorId { get; set; }

    public int AuditoriaId { get; set; }
    public Auditoria? Auditoria { get; set; }

    [Required, StringLength(100)]
    public string AuditorEvaluadoId { get; set; } = string.Empty;

    [Required, StringLength(50)]
    public string Estatus { get; set; } = "Pendiente"; // Pendiente / Completada

    public DateTime FechaCreacion { get; set; } = DateTime.Now;

    // Capturados por SGC al llenar la evaluación.
    public DateTime? FechaEvaluacion { get; set; }

    [StringLength(300)]
    public string? ResponsablesEvaluacion { get; set; }

    [StringLength(100)]
    public string? TotalHorasAuditadas { get; set; }

    // Porcentaje 0-100: con 20 preguntas de 1-5, la suma de calificaciones ya es el %.
    [Column(TypeName = "decimal(5,2)")]
    public decimal? CalificacionGeneral { get; set; }

    [StringLength(100)]
    public string? CompletadaPorId { get; set; }

    public DateTime? FechaCompletada { get; set; }

    public ICollection<EvaluacionAuditorRespuesta> Respuestas { get; set; } = new List<EvaluacionAuditorRespuesta>();
}
