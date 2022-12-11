namespace KeyManager.Application.Commands.UserRoles;

public class CreateUserRoleCommand : IRequest<UserRoleDto>
{
    public CreateUserRoleCommand(long userId, long roleId)
    {
        UserId = userId;
        RoleId = roleId;
    }

    [Required(ErrorMessage = "UserId is required.")]
    public long UserId { get; }

    [Required(ErrorMessage = "RoleId is required.")]
    public long RoleId { get; }
}