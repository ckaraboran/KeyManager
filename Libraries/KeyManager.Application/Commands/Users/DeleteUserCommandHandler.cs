namespace KeyManager.Application.Commands.Users;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand>
{
    private readonly IGenericRepository<User> _userRepository;

    public DeleteUserCommandHandler(IGenericRepository<User> userRepository)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    }

    public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.Id);

        if (user == null) throw new UserException($"User not found while deleting. UserId: '{request.Id}'");

        await _userRepository.DeleteAsync(user);
        return Unit.Value;
    }
}