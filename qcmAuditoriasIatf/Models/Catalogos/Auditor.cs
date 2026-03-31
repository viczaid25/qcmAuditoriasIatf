using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace qcmAuditoriasIatf.Models.Catalogos;

[Table("qmcAudAuditor")]
public class Auditor
{
    [Key]
    public int AuditorId { get; set; }

    // referencia a meax_all_user (elige 1: id int o pc_login_id string)
    public int? MeaxUserId { get; set; }

    [Required, StringLength(200)]
    public string Nombre { get; set; } = string.Empty;

    [StringLength(200)]
    public string? Funcion { get; set; } // position / rol

    [Required, StringLength(50)]
    public string Tipo { get; set; } = "Auditor"; // Auditor / Entrenamiento / Observador

    [StringLength(150)]
    public string? Departamento { get; set; }

    [StringLength(200)]
    public string? Email { get; set; }

    [StringLength(100)]
    public string? PcLoginId { get; set; }

    public bool Activo { get; set; } = true;
}