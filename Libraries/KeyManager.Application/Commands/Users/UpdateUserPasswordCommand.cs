namespace KeyManager.Application.Commands.Users;

public class UpdateUserPasswordCommand : IRequest
{
    public UpdateUserPasswordCommand(string username, string oldPassword, string newPassword)
    {
        Username = username;
        OldPassword = oldPassword;
        NewPassword = newPassword;
    }

    [Required(ErrorMessage = "Username is required.")]
    public string Username { get; }

    [Required(ErrorMessage = "OldPassword is required.")]
    public string OldPassword { get; }

    [Required(ErrorMessage = "NewPassword is required.")]
    public string NewPassword { get; }
}