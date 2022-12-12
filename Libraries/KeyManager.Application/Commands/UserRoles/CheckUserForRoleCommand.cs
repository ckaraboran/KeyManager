namespace KeyManager.Application.Commands.UserRoles;

public class CheckUserForRoleCommand: IRequest<bool>
{
    public CheckUserForRoleCommand(string username, string rolename)
    {
        UserName = username;
        RoleName = rolename;
    }

    [Required(ErrorMessage = "Username is required.")]
    public string UserName { get; }

    [Required(ErrorMessage = "RoleName is required.")]
    public string RoleName { get; }
}