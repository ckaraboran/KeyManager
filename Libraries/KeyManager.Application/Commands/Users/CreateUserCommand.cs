namespace KeyManager.Application.Commands.Users;

public class CreateUserCommand : IRequest<UserDto>
{
    public CreateUserCommand(long employeeId, string name, string surname)
    {
        EmployeeId = employeeId;
        Name = name;
        Surname = surname;
    }

    public long EmployeeId { get; }

    [Required(ErrorMessage = "Name is required.")]
    public string Name { get; }

    [Required(ErrorMessage = "Surname is required.")]
    public string Surname { get; }
}