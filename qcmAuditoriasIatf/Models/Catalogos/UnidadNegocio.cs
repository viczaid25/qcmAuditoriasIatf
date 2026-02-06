using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace qcmAuditoriasIatf.Models.Catalogos;

[Table("qmcAudUnidadNegocio")]
public class UnidadNegocio
{
    [Key]
    public int UnidadNegocioId { get; set; }

    [Required]
    public int ProcesoId { get; set; }

    // opcional: clave corta para UI/reportes (ALT, MAR, EPS, etc.)
    [Required, StringLength(30)]
    public string Codigo { get; set; } = string.Empty;

    [Required, StringLength(200)]
    public string Nombre { get; set; } = string.Empty;

    public bool Activo { get; set; } = true;

    // navegación
    public Proceso? Proceso { get; set; }
}