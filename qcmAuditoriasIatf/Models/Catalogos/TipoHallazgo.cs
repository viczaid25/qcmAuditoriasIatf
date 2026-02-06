using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using qcmAuditoriasIatf.Models.Hallazgos;

namespace qcmAuditoriasIatf.Models.Catalogos;

[Table("qmcAudTipoHallazgo")]
public class TipoHallazgo
{
    [Key]
    public int TipoHallazgoId { get; set; }

    [Required]
    public string Nombre { get; set; } = string.Empty; // NC Mayor, NC Menor, Observación...

    public int Severidad { get; set; }

    public ICollection<Hallazgo> Hallazgos { get; set; } = new List<Hallazgo>();
}