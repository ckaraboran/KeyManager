namespace KeyManager.Application.Commands.Permissions;

public class UpdatePermissionCommandHandler : IRequestHandler<UpdatePermissionCommand, PermissionDto>
{
    private readonly IGenericRepository<Door> _doorRepository;
    private readonly IMapper _mapper;
    private readonly IGenericRepository<Permission> _permissionRepository;
    private readonly IGenericRepository<User> _userRepository;

    public UpdatePermissionCommandHandler(IGenericRepository<Permission> permissionRepository,
        IGenericRepository<User> userRepository, IGenericRepository<Door> doorRepository,
        IMapper mapper)
    {
        _permissionRepository = permissionRepository ?? throw new ArgumentNullException(nameof(permissionRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _doorRepository = doorRepository ?? throw new ArgumentNullException(nameof(doorRepository));
    }

    public async Task<PermissionDto> Handle(UpdatePermissionCommand request, CancellationToken cancellationToken)
    {
        var existingPermission = await _permissionRepository.GetByIdAsync(request.Id);

        if (existingPermission == null)
            throw new RecordNotFoundException($"Permission not found. PermissionId: '{request.Id}'");

        var existingPermissionWithSameValues = await _permissionRepository
            .GetAsync(s => s.UserId == request.UserId && s.DoorId == request.DoorId && s.Id != request.Id);

        if (existingPermissionWithSameValues != null)
            throw new RecordAlreadyExistsException("There is a permission with the same User ID and Door ID: " +
                                                   $"User ID: '{request.UserId}', Door ID: '{request.DoorId}'");

        var user = await _userRepository.GetByIdAsync(request.UserId);
        if (user == null) throw new RecordNotFoundException($"User not found. User ID: '{request.UserId}'");

        var door = await _doorRepository.GetByIdAsync(request.DoorId);
        if (door == null) throw new RecordNotFoundException($"Door not found. Door ID: '{request.DoorId}'");

        existingPermission = _mapper.Map<Permission>(request);
        var updatedPermission = await _permissionRepository.UpdateAsync(existingPermission);

        return _mapper.Map<PermissionDto>(updatedPermission);
    }
}