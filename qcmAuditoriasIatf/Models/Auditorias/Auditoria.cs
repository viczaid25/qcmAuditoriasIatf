using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using qcmAuditoriasIatf.Models.Hallazgos;

namespace qcmAuditoriasIatf.Models.Auditorias;

[Table("qmcAudAuditoria")]
public class Auditoria
{
    [Key]
    public int AuditoriaId { get; set; }

    public DateTime FechaInicio { get; set; } = DateTime.Today;
    public DateTime FechaFin { get; set; } = DateTime.Today;

    [Required]
    public string TipoAuditoria { get; set; } = "Interna";

    [Required]
    public string Objetivo { get; set; } = string.Empty;

    [Required]
    public string Alcance { get; set; } = string.Empty;

    [Required]
    public string AuditorLiderId { get; set; } = "sistema"; // temporal si no tienes Identity

    [Required]
    public string Estatus { get; set; } = "Planeada"; // Planeada/EnProceso/Cerrada

    public ICollection<AuditoriaProceso> AuditoriaProcesos { get; set; } = new List<AuditoriaProceso>();
    public ICollection<AuditoriaChecklist> AuditoriaChecklists { get; set; } = new List<AuditoriaChecklist>();
    public ICollection<Hallazgo> Hallazgos { get; set; } = new List<Hallazgo>();
}