using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace qcmAuditoriasIatf.Models.Auditorias;

// Respuesta a una de las 20 preguntas fijas del formato MX-6100-F3 (texto y
// categoría de cada pregunta se definen en código, no en catálogo, por ser un
// formato controlado que rara vez cambia).
[Table("qmcAudEvaluacionAuditorRespuesta")]
public class EvaluacionAuditorRespuesta
{
    [Key]
    public int EvaluacionAuditorRespuestaId { get; set; }

    public int EvaluacionAuditorId { get; set; }
    public EvaluacionAuditor? Evaluacion { get; set; }

    public int NumeroPregunta { get; set; }

    public int? Calificacion { get; set; } // 1-5

    [StringLength(1000)]
    public string? Comentario { get; set; }
}
