namespace KeyManager.Application.Commands.Permissions;

public class UpdatePermissionCommand : IRequest<PermissionDto>
{
    public UpdatePermissionCommand(long id, long userId, long doorId)
    {
        Id = id;
        UserId = userId;
        DoorId = doorId;
    }

    [Required(ErrorMessage = "Id is required.")]
    public long Id { get; }

    [Required(ErrorMessage = "UserId is required.")]
    public long UserId { get; }

    [Required(ErrorMessage = "DoorId is required.")]
    public long DoorId { get; }
}