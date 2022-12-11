namespace KeyManager.Application.Commands.Users;

public class UpdateUserCommand : IRequest<UserDto>
{
    public UpdateUserCommand(long id, string username, string name, string surname)
    {
        Id = id;
        Username = username;
        Name = name;
        Surname = surname;
    }

    [Required(ErrorMessage = "Id is required.")]
    public long Id { get; }

    [Required(ErrorMessage = "Username is required.")]
    public string Username { get; }

    [Required(ErrorMessage = "Name is required.")]
    public string Name { get; }

    [Required(ErrorMessage = "Surname is required.")]
    public string Surname { get; }
}