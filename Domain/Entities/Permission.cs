using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Permission
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string EmployeeForeName { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string EmployeeSurName { get; set; } = string.Empty;

    [Required]
    public int PermissionTypeId { get; set; }

    [Required]
    public DateTime PermissionDate { get; set; }

    [ForeignKey("PermissionTypeId")]
    public PermissionType? PermissionType { get; set; }
}
