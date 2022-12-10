namespace KeyManager.Application.Commands.Users;

public class DeleteUserCommand : IRequest
{
    public DeleteUserCommand(long id)
    {
        Id = id;
    }

    [Required(ErrorMessage = "Id is required.")]
    public long Id { get; }
}