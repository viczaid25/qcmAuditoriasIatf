using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace qcmAuditoriasIatf.Models.Catalogos;

[Table("qmcAudClausulaIATF")]
public class ClausulaIATF
{
    [Key]
    public int ClausulaId { get; set; }

    [Required]
    public string Codigo { get; set; } = string.Empty; // ej: 8.5.1, 8.5.2.1

    [Required]
    public string Descripcion { get; set; } = string.Empty;

    public bool Activo { get; set; } = true;

    public ICollection<ChecklistPregunta> ChecklistPreguntas { get; set; } = new List<ChecklistPregunta>();
}