namespace KeyManager.Application.Commands.Users;

public class CreateUserCommand : IRequest<UserDto>
{
    public CreateUserCommand(string username, string name, string surname)
    {
        Username = username;
        Name = name;
        Surname = surname;
    }

    [Required(ErrorMessage = "Username is required.")]
    public string Username { get; }

    [Required(ErrorMessage = "Name is required.")]
    public string Name { get; }

    [Required(ErrorMessage = "Surname is required.")]
    public string Surname { get; }
}