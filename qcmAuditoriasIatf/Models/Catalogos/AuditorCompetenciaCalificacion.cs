using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace qcmAuditoriasIatf.Models.Catalogos;

// Marca si un auditor cuenta con una competencia de la matriz (MX-6100-F7).
// Simplificado a Sí/No (en vez de la escala 1/3/6 del formato original): la
// evidencia real de la calificación es el certificado adjunto (ver Evidencia,
// ReferenciaTipo="CompetenciaAuditor", ReferenciaId=AuditorCompetenciaId).
[Table("qmcAudAuditorCompetencia")]
public class AuditorCompetenciaCalificacion
{
    [Key]
    public int AuditorCompetenciaId { get; set; }

    public int AuditorId { get; set; }
    public Auditor? Auditor { get; set; }

    public int CompetenciaAuditorId { get; set; }
    public CompetenciaAuditor? Competencia { get; set; }

    public bool TieneCompetencia { get; set; }

    public DateTime? FechaActualizacion { get; set; }

    [StringLength(100)]
    public string? ActualizadoPorId { get; set; }
}
