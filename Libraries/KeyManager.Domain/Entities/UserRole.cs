using System.ComponentModel.DataAnnotations.Schema;

namespace KeyManager.Domain.Entities;

public class UserRole : BaseEntity
{
    [ForeignKey(nameof(User))] public long UserId { get; set; }

    public User User { get; set; }

    [ForeignKey(nameof(Role))] public long RoleId { get; set; }

    public Role Role { get; set; }
}