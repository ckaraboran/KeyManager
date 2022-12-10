namespace KeyManager.Application.Commands.Doors;

public class DeleteDoorCommand : IRequest
{
    public DeleteDoorCommand(long id)
    {
        Id = id;
    }

    [Required(ErrorMessage = "Id is required.")]
    public long Id { get; }
}