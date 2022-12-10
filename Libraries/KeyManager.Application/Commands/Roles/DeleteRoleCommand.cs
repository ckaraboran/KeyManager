namespace KeyManager.Application.Commands.Roles;

public class DeleteRoleCommand : IRequest
{
    public DeleteRoleCommand(int id)
    {
        Id = id;
    }

    [Required(ErrorMessage = "Id is required.")]
    public int Id { get; }
}