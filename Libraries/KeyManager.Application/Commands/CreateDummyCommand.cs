using System.ComponentModel.DataAnnotations;
using MediatR;

namespace KeyManager.Application.Commands;

public class CreateDummyCommand : IRequest<DummyDto>
{
    [Required(ErrorMessage = "Name is required.")]
    public string Name { get; private set; }

    public CreateDummyCommand(string name)
    {
        Name = name;
    }
}