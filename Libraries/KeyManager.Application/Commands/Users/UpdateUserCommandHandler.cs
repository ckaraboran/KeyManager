namespace KeyManager.Application.Commands.Users;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UserDto>
{
    private readonly IMapper _mapper;
    private readonly IGenericRepository<User> _userRepository;

    public UpdateUserCommandHandler(IGenericRepository<User> userRepository, IMapper mapper)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<UserDto> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _userRepository.GetByIdAsync(request.Id);

        if (existingUser == null) throw new UserException($"User is not found. UserId: '{request.Id}'");

        if (existingUser.EmployeeId != request.EmployeeId)
            throw new UserException("Employee ID cannot be different then current one. " +
                                    $"EmployeeId: '{request.EmployeeId}'");

        existingUser = _mapper.Map<User>(request);
        var updatedUser = await _userRepository.UpdateAsync(existingUser);

        return _mapper.Map<UserDto>(updatedUser);
    }
}