using KeyManager.Infrastructure.Security;

namespace KeyManager.Application.Commands.Users;

public class UpdateUserPasswordCommandHandler : IRequestHandler<UpdateUserPasswordCommand>
{
    private readonly IGenericRepository<User> _userRepository;

    public UpdateUserPasswordCommandHandler(IGenericRepository<User> userRepository)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    }

    public async Task<Unit> Handle(UpdateUserPasswordCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _userRepository.GetAsync(x => x.Username == request.Username);

        var oldPasswordCorrect = ClayPasswordHasher
            .IsSameWithHashedPassword(existingUser, existingUser.Password, request.OldPassword);
        if (!oldPasswordCorrect) throw new UserException("Old password is wrong.");

        existingUser.Password = ClayPasswordHasher.HashPassword(existingUser, request.NewPassword);
        await _userRepository.UpdateAsync(existingUser);

        return Unit.Value;
    }
}