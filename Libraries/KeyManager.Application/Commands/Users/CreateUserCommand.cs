namespace KeyManager.Application.Commands.Users;

public class CreateUserCommand : IRequest<UserDto>
{
    public CreateUserCommand(string username, string name, string surname, string password)
    {
        Username = username;
        Name = name;
        Surname = surname;
        Password = password;
    }

    [Required(ErrorMessage = "Username is required.")]
    public string Username { get; }

    [Required(ErrorMessage = "Name is required.")]
    public string Name { get; }

    [Required(ErrorMessage = "Surname is required.")]
    public string Surname { get; }

    [Required(ErrorMessage = "Password is required.")]
    public string Password { get; }
}