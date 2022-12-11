namespace KeyManager.Application.Commands.Permissions;

public class DeletePermissionCommand : IRequest
{
    public DeletePermissionCommand(long id)
    {
        Id = id;
    }

    [Required(ErrorMessage = "Id is required.")]
    public long Id { get; }
}