namespace KeyManager.Application.Commands.Doors;

public class OpenDoorCommand : IRequest<bool>
{
    public OpenDoorCommand(string username, long doorId)
    {
        Username = username;
        DoorId = doorId;
    }

    [Required(ErrorMessage = "Username is required.")]
    public string Username { get; }

    [Required(ErrorMessage = "DoorId is required.")]
    public long DoorId { get; }
}