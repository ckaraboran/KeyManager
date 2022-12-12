using KeyManager.Infrastructure.Security;

namespace KeyManager.Application.Commands.Users;

public class CheckAuthenticationCommandHandler : IRequestHandler<CheckAuthenticationCommand, bool>
{
    private readonly IGenericRepository<User> _userRepository;

    public CheckAuthenticationCommandHandler(IGenericRepository<User> userRepository)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    }

    public async Task<bool> Handle(CheckAuthenticationCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetAsync(x => x.Username == request.Username);
        if (user == null) throw new UserException($"User not found. Username: '{request.Username}'");

        var isUserOk =
            ClayPasswordHasher.IsPasswordOk(user, user.Password, request.Password);
        return isUserOk;
    }
}