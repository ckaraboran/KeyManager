namespace KeyManager.Api.DTOs.Responses.UserRoles;

public class CreateUserRoleResponse
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public long RoleId { get; set; }
}