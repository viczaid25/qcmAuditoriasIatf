using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace qcmAuditoriasIatf.Models.Catalogos;

// Catálogo de las competencias evaluadas en la matriz de auditores internos
// (formato MX-6100-F7). Las marcadas con Obligatoria son mandatorias (**) para
// poder ser auditor interno o auditor líder según el formato.
[Table("qmcAudCompetenciaAuditor")]
public class CompetenciaAuditor
{
    [Key]
    public int CompetenciaAuditorId { get; set; }

    [Required, StringLength(150)]
    public string Nombre { get; set; } = string.Empty;

    // Agrupación visual de la matriz, ej. "Conocimientos específicos" / "Conocimientos CSR".
    [StringLength(80)]
    public string? Grupo { get; set; }

    public bool Obligatoria { get; set; }

    public int Orden { get; set; }

    public bool Activo { get; set; } = true;

    public ICollection<AuditorCompetenciaCalificacion> Calificaciones { get; set; } = new List<AuditorCompetenciaCalificacion>();
}
