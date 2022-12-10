namespace KeyManager.Application.Commands.Doors;

public class UpdateDoorCommand : IRequest<DoorDto>
{
    public UpdateDoorCommand(int id, string name)
    {
        Id = id;
        Name = name;
    }

    [Required(ErrorMessage = "Id is required.")]
    public long Id { get; }

    [Required(ErrorMessage = "Name is required.")]
    public string Name { get; }
}