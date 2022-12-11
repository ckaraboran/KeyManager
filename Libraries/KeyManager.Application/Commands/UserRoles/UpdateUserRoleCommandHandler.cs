namespace KeyManager.Application.Commands.UserRoles;

public class UpdateUserRoleCommandHandler : IRequestHandler<UpdateUserRoleCommand, UserRoleDto>
{
    private readonly IMapper _mapper;
    private readonly IGenericRepository<Role> _roleRepository;
    private readonly IGenericRepository<User> _userRepository;
    private readonly IGenericRepository<UserRole> _userRoleRepository;

    public UpdateUserRoleCommandHandler(IGenericRepository<UserRole> userRoleRepository,
        IGenericRepository<User> userRepository, IGenericRepository<Role> roleRepository,
        IMapper mapper)
    {
        _userRoleRepository = userRoleRepository ?? throw new ArgumentNullException(nameof(userRoleRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _roleRepository = roleRepository ?? throw new ArgumentNullException(nameof(roleRepository));
    }

    public async Task<UserRoleDto> Handle(UpdateUserRoleCommand request, CancellationToken cancellationToken)
    {
        var existingUserRole = await _userRoleRepository.GetByIdAsync(request.Id);

        if (existingUserRole == null)
            throw new UserRoleException($"UserRole not found. UserRoleId: '{request.Id}'");

        var existingUserRoleWithSameValues = await _userRoleRepository
            .GetAsync(s => s.UserId == request.UserId && s.RoleId == request.RoleId && s.Id != request.Id);

        if (existingUserRoleWithSameValues != null)
            throw new UserRoleException("There is a userRole with the same User ID and Role ID: " +
                                        $"User ID: '{request.UserId}', Role ID: '{request.RoleId}'");

        var user = await _userRepository.GetByIdAsync(request.UserId);
        if (user == null) throw new UserRoleException($"User not found. User ID: '{request.UserId}'");

        var role = await _roleRepository.GetByIdAsync(request.RoleId);
        if (role == null) throw new UserRoleException($"Role not found. Role ID: '{request.RoleId}'");

        existingUserRole = _mapper.Map<UserRole>(request);
        var updatedUserRole = await _userRoleRepository.UpdateAsync(existingUserRole);

        return _mapper.Map<UserRoleDto>(updatedUserRole);
    }
}