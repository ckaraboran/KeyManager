namespace KeyManager.Application.Commands.Users;

public class CreateUserCommand : IRequest<UserDto>
{
    public CreateUserCommand(long employeeId, string name, string surname, long roleId)
    {
        EmployeeId = employeeId;
        Name = name;
        Surname = surname;
        RoleId = roleId;
    }

    public long EmployeeId { get; }

    [Required(ErrorMessage = "Name is required.")]
    public string Name { get; }

    [Required(ErrorMessage = "Surname is required.")]
    public string Surname { get; }

    public long RoleId { get; }
}