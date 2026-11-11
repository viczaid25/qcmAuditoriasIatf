using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace qcmAuditoriasIatf.Models.Auditorias;

// Persona a la que SGC le pidió revisar el informe extendido (Misconduct) de una auditoría.
// La confirmación guarda una huella del contenido que vio (HashContenido): si el informe
// cambia después, la revisión queda como "desactualizada" y hay que volver a pedirla.
[Table("qmcAudInformeMisconductRevision")]
public class InformeMisconductRevision
{
    [Key]
    public int InformeMisconductRevisionId { get; set; }

    public int AuditoriaId { get; set; }
    public Auditoria? Auditoria { get; set; }

    [Required, StringLength(100)]
    public string RevisorId { get; set; } = string.Empty;

    [StringLength(100)]
    public string? SolicitadoPorId { get; set; }

    public DateTime FechaSolicitud { get; set; } = DateTime.Now;

    public bool Confirmada { get; set; }

    public DateTime? FechaConfirmacion { get; set; }

    [StringLength(64)]
    public string? HashContenido { get; set; }
}
