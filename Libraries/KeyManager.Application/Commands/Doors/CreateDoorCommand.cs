using System.ComponentModel.DataAnnotations;
using MediatR;

namespace KeyManager.Application.Commands.Doors;

public class CreateDoorCommand : IRequest<DoorDto>
{
    public CreateDoorCommand(string name)
    {
        Name = name;
    }

    [Required(ErrorMessage = "Name is required.")]
    public string Name { get; }
}