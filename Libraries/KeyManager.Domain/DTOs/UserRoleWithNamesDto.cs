namespace KeyManager.Domain.DTOs;

public class UserRoleWithNamesDto
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public string UserName { get; set; }
    public long RoleId { get; set; }
    public string RoleName { get; set; }
}