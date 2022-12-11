namespace KeyManager.Application.Commands.UserRoles;

public class UpdateUserRoleCommand : IRequest<UserRoleDto>
{
    public UpdateUserRoleCommand(long id, long userId, long roleId)
    {
        Id = id;
        UserId = userId;
        RoleId = roleId;
    }

    [Required(ErrorMessage = "Id is required.")]
    public long Id { get; }

    [Required(ErrorMessage = "UserId is required.")]
    public long UserId { get; }

    [Required(ErrorMessage = "RoleId is required.")]
    public long RoleId { get; }
}