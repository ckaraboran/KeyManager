namespace KeyManager.Application.Commands.Users;

public class CheckAuthenticationCommand : IRequest<bool>
{
    public CheckAuthenticationCommand(string username, string password)
    {
        Username = username;
        Password = password;
    }

    [Required(ErrorMessage = "Username is required.")]
    public string Username { get; }

    [Required(ErrorMessage = "Password is required.")]
    public string Password { get; }
}