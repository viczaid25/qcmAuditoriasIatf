using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using qcmAuditoriasIatf.Models.Auditorias;
using qcmAuditoriasIatf.Models.Catalogos;

namespace qcmAuditoriasIatf.Models.Hallazgos;

[Table("qmcAudHallazgo")]
public class Hallazgo
{
    [Key]
    public int HallazgoId { get; set; }

    public int AuditoriaId { get; set; }
    public Auditoria? Auditoria { get; set; }

    public int ProcesoId { get; set; }
    public Proceso? Proceso { get; set; }

    public int ClausulaId { get; set; }
    public ClausulaIATF? Clausula { get; set; }

    public int TipoHallazgoId { get; set; }
    public TipoHallazgo? TipoHallazgo { get; set; }

    [Required]
    public string Descripcion { get; set; } = string.Empty;

    public string? Evidencia { get; set; }

    [Required]
    public string ResponsableId { get; set; } = "sistema";

    [StringLength(100)]
    public string? ResponsableCierreId { get; set; }

    public DateTime FechaCompromiso { get; set; } = DateTime.Today.AddDays(7);

    [Required]
    public string Estatus { get; set; } = "Abierto"; // Abierto/Cerrado

    public int AuditoriaProcesoId { get; set; }
    public AuditoriaProceso? AuditoriaProceso { get; set; }

    public int? PreguntaId { get; set; }
    public ChecklistPregunta? Pregunta { get; set; }

    public ICollection<AccionCorrectiva> AccionesCorrectivas { get; set; } = new List<AccionCorrectiva>();
    public ICollection<HallazgoSeguimiento> Seguimientos { get; set; } = new List<HallazgoSeguimiento>();
    public HallazgoCincoPorQue? CincoPorQue { get; set; }
    [StringLength(100)]
    public string? CreadoPorId { get; set; }

    public DateTime FechaCreacion { get; set; } = DateTime.Now;

    [Required, StringLength(50)]
    public string EstatusValidacionSgc { get; set; } = "Pendiente";
    // Pendiente / Aprobado / Rechazado

    [StringLength(100)]
    public string? RevisadoPorSgcId { get; set; }

    public DateTime? FechaRevisionSgc { get; set; }

    [StringLength(2000)]
    public string? ComentarioRevisionSgc { get; set; }

}