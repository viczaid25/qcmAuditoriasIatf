using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace qcmAuditoriasIatf.Models.External;

[Table("meax_all_user")]
public class MeaxAllUser
{
    [Key]
    public int id { get; set; }

    [StringLength(100)]
    public string pc_login_id { get; set; } = string.Empty;

    [StringLength(200)]
    public string? username { get; set; }

    [StringLength(200)]
    public string? email { get; set; }

    [StringLength(100)]
    public string? department { get; set; }

    [StringLength(100)]
    public string? position { get; set; }

    [StringLength(100)]
    public string? dep_2 { get; set; }

    // ✅ tinyint -> byte
    public byte auth { get; set; }
}