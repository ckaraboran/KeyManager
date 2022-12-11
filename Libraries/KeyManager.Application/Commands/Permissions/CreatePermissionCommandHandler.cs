namespace KeyManager.Application.Commands.Permissions;

public class CreatePermissionCommandHandler : IRequestHandler<CreatePermissionCommand, PermissionDto>
{
    private readonly IGenericRepository<Door> _doorRepository;
    private readonly IMapper _mapper;
    private readonly IGenericRepository<Permission> _permissionRepository;
    private readonly IGenericRepository<User> _userRepository;

    public CreatePermissionCommandHandler(IGenericRepository<Permission> permissionRepository,
        IGenericRepository<User> userRepository, IGenericRepository<Door> doorRepository,
        IMapper mapper)
    {
        _permissionRepository = permissionRepository ?? throw new ArgumentNullException(nameof(permissionRepository));
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _doorRepository = doorRepository ?? throw new ArgumentNullException(nameof(doorRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<PermissionDto> Handle(CreatePermissionCommand request, CancellationToken cancellationToken)
    {
        var existingPermission = await _permissionRepository
            .GetAsync(s => s.UserId == request.UserId && s.DoorId == request.DoorId);

        if (existingPermission != null)
            throw new PermissionException("There is a permission with the same User ID and Door ID: " +
                                          $"User ID: '{request.UserId}', Door ID: '{request.DoorId}'");

        var user = await _userRepository.GetByIdAsync(request.UserId);
        if (user == null) throw new PermissionException($"User not found. User ID: '{request.UserId}'");

        var door = await _doorRepository.GetByIdAsync(request.DoorId);
        if (door == null) throw new PermissionException($"Door not found. Door ID: '{request.DoorId}'");

        var permission = await _permissionRepository.AddAsync(_mapper.Map<Permission>(request));

        return _mapper.Map<PermissionDto>(permission);
    }
}