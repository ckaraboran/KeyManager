namespace KeyManager.Application.Commands.Doors;

public class OpenDoorCommand : IRequest<bool>
{
    public OpenDoorCommand(long userId, long doorId)
    {
        UserId = userId;
        DoorId = doorId;
    }

    [Required(ErrorMessage = "UserId is required.")]
    public long UserId { get; }

    [Required(ErrorMessage = "DoorId is required.")]
    public long DoorId { get; }
}