namespace KeyManager.Application.Commands.Roles;

public class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommand, RoleDto>
{
    private readonly IMapper _mapper;
    private readonly IGenericRepository<Role> _roleRepository;

    public UpdateRoleCommandHandler(IGenericRepository<Role> roleRepository, IMapper mapper)
    {
        _roleRepository = roleRepository ?? throw new ArgumentNullException(nameof(roleRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<RoleDto> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
    {
        var existingRole = await _roleRepository.GetAsync(request.Id);

        if (existingRole == null)
            throw new RoleException($"Role already exists. RoleId: '{request.Id}'");

        existingRole = _mapper.Map<Role>(request);
        var updatedRole = await _roleRepository.UpdateAsync(existingRole);

        return _mapper.Map<RoleDto>(updatedRole);
    }
}