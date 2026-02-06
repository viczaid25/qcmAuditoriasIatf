using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using qcmAuditoriasIatf.Models.Auditorias;

namespace qcmAuditoriasIatf.Models.Catalogos;

[Table("qmcAudProceso")]
public class Proceso
{
    [Key]
    public int ProcesoId { get; set; }

    [Required, StringLength(20)]
    public string Codigo { get; set; } = string.Empty; // MX-0100-D1

    [Required, StringLength(200)]
    public string Nombre { get; set; } = string.Empty; // ALTA DIRECCIÓN

    public string? Descripcion { get; set; }

    [Required]
    public string Area { get; set; } = "QMS";

    // ✅ Para Producción por unidad + captura de línea
    [StringLength(80)]
    public string? UnidadNegocio { get; set; } // Alternador, Marchas, etc.


    [StringLength(120)]
    public string? LineaNombre { get; set; } // nombre manual de línea

    public bool Activo { get; set; } = true;

    public ICollection<AuditoriaProceso> AuditoriaProcesos { get; set; } = new List<AuditoriaProceso>();
}