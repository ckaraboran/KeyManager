namespace KeyManager.Application.Commands.Permissions;

public class CreatePermissionCommand : IRequest<PermissionDto>
{
    public CreatePermissionCommand(long userId, long doorId)
    {
        UserId = userId;
        DoorId = doorId;
    }

    [Required(ErrorMessage = "UserId is required.")]
    public long UserId { get; }

    [Required(ErrorMessage = "DoorId is required.")]
    public long DoorId { get; }
}