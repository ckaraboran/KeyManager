using System.ComponentModel.DataAnnotations.Schema;

namespace KeyManager.Domain.Entities;

public class Permission: BaseEntity
{
    [ForeignKey(nameof(Role))]
    public long RoleId { get; set; }
    public Role Role { get; set; }
    [ForeignKey(nameof(Door))]
    public long DoorId { get; set; }
    public Door Door { get; set; }
}