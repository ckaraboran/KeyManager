namespace KeyManager.Api.DTOs.Responses.UserRoles;

public class GetUserRoleResponse
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public string UserName { get; set; }
    public long RoleId { get; set; }
    public string RoleName { get; set; }
}