namespace KeyManager.Application.Commands.Users;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserDto>
{
    private readonly IMapper _mapper;
    private readonly IGenericRepository<User> _userRepository;


    public CreateUserCommandHandler(IGenericRepository<User> userRepository, IMapper mapper)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var existingEmployee = await _userRepository
            .GetAsync(s => s.EmployeeId == request.EmployeeId);

        if (existingEmployee != null)
            throw new UserException($"There is a user with the same employee ID: '{request.Name}'");

        var dummy = await _userRepository.AddAsync(_mapper.Map<User>(request));

        return _mapper.Map<UserDto>(dummy);
    }
}