namespace KeyManager.Application.Commands.Users;

public class UpdateUserCommand : IRequest<UserDto>
{
    public UpdateUserCommand(long id, long employeeId, string name, string surname, long roleId)
    {
        Id = id;
        EmployeeId = employeeId;
        Name = name;
        Surname = surname;
        RoleId = roleId;
    }

    [Required(ErrorMessage = "Id is required.")]
    public long Id { get; }

    public long EmployeeId { get; }

    [Required(ErrorMessage = "Name is required.")]
    public string Name { get; }

    [Required(ErrorMessage = "Surname is required.")]
    public string Surname { get; }

    public long RoleId { get; }
}