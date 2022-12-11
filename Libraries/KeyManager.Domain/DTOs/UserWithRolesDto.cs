namespace KeyManager.Domain.DTOs;

public class UserWithRolesDto
{
    public string Username { get; set; }
    public List<string> RoleNames { get; set; }
}