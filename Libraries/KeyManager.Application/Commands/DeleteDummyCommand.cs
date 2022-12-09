using System.ComponentModel.DataAnnotations;
using MediatR;

namespace KeyManager.Application.Commands;

public class DeleteDummyCommand : IRequest
{
    public DeleteDummyCommand(int id)
    {
        Id = id;
    }

    [Required(ErrorMessage = "Id is required.")]
    public int Id { get; }
}