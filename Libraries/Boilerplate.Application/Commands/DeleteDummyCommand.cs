using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Boilerplate.Application.Commands;

public class DeleteDummyCommand : IRequest
{
    public DeleteDummyCommand(int id)
    {
        Id = id;
    }

    [Required(ErrorMessage = "Id is required.")]
    public int Id { get; }
}