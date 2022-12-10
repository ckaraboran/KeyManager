namespace KeyManager.Domain.Entities;

public class User: BaseEntity
{
    public long EmployeeId { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public long RoleId { get; set; }
    public Role Role { get; set; }
}