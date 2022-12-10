namespace KeyManager.Application.Commands.Roles;

public class DeleteRoleCommandHandler : IRequestHandler<DeleteRoleCommand>
{
    private readonly IGenericRepository<Role> _roleRepository;

    public DeleteRoleCommandHandler(IGenericRepository<Role> roleRepository)
    {
        _roleRepository = roleRepository ?? throw new ArgumentNullException(nameof(roleRepository));
    }

    public async Task<Unit> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        var role = await _roleRepository.GetAsync(request.Id);

        if (role == null) throw new RoleException($"Role is not found while deleting. RoleId: '{request.Id}'");

        await _roleRepository.DeleteAsync(role);
        return Unit.Value;
    }
}