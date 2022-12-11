namespace KeyManager.Application.Commands.UserRoles;

public class DeleteUserRoleCommand : IRequest
{
    public DeleteUserRoleCommand(long id)
    {
        Id = id;
    }

    [Required(ErrorMessage = "Id is required.")]
    public long Id { get; }
}