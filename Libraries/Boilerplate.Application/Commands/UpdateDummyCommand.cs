using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Boilerplate.Application.Commands;

public class UpdateDummyCommand : IRequest<DummyDto>
{
    public UpdateDummyCommand(int id, string name)
    {
        Id = id;
        Name = name;
    }

    [Required(ErrorMessage = "Id is required.")]
    public int Id { get; }

    [Required(ErrorMessage = "Name is required.")]
    public string Name { get; }
}