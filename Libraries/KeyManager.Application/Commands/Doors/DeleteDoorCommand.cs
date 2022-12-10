using System.ComponentModel.DataAnnotations;
using MediatR;

namespace KeyManager.Application.Commands.Doors;

public class DeleteDoorCommand : IRequest
{
    public DeleteDoorCommand(int id)
    {
        Id = id;
    }

    [Required(ErrorMessage = "Id is required.")]
    public int Id { get; }
}