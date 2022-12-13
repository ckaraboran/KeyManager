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
            .GetAsync(s => s.Username == request.Username);

        if (existingEmployee != null)
            throw new RecordAlreadyExistsException($"There is a user with the same username: '{request.Username}'");

        var user = await _userRepository.AddAsync(_mapper.Map<User>(request));

        return _mapper.Map<UserDto>(user);
    }
}