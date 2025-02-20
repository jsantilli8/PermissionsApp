using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class PermissionType
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(250)]
    public string Description { get; set; } = string.Empty;

    public ICollection<Permission> Permissions { get; set; } = new List<Permission>();
}
