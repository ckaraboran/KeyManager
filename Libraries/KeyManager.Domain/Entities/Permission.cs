namespace KeyManager.Domain.Entities;

public class Permission: BaseEntity
{
    public long RoleId { get; set; }
    public Role Role { get; set; }
    public long DoorId { get; set; }
    public Door Door { get; set; }
}