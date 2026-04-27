using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace qcmAuditoriasIatf.Models.Seguridad;

[Table("qmcAudUsuarioPerfil")]
public class UsuarioPerfil
{
    [Key]
    public int UsuarioPerfilId { get; set; }

    [Required, StringLength(100)]
    public string PcLoginId { get; set; } = string.Empty;

    [Required, StringLength(50)]
    public string Perfil { get; set; } = string.Empty;
    // SGC, Administrador, Consulta, etc.

    public bool Activo { get; set; } = true;
}