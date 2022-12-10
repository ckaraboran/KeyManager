using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KeyManager.Domain.Entities;

public class User: BaseEntity
{
    public long EmployeeId { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string Surname { get; set; }
    [ForeignKey(nameof(Role))]
    public long RoleId { get; set; }
    public Role Role { get; set; }
}