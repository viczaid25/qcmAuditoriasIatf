using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using qcmAuditoriasIatf.Models.Auditorias;

namespace qcmAuditoriasIatf.Models.Catalogos;

[Table("qmcAudChecklistPregunta")]
public class ChecklistPregunta
{
    [Key]
    public int PreguntaId { get; set; }

    public int ClausulaId { get; set; }
    public ClausulaIATF? Clausula { get; set; }

    public int ProcesoId { get; set; }
    public Proceso? Proceso { get; set; }

    [Required]
    public string Pregunta { get; set; } = string.Empty;

    public string? Requisito { get; set; }

    public bool Activo { get; set; } = true;

    public ICollection<AuditoriaChecklist> AuditoriaChecklists { get; set; } = new List<AuditoriaChecklist>();
}