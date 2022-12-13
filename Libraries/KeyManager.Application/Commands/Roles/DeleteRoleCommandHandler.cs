using KeyManager.Domain.Enums;

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
        var role = await _roleRepository.GetByIdAsync(request.Id);

        if (role == null) throw new RecordNotFoundException($"Role not found while deleting. RoleId: '{request.Id}'");

        if (Enum.IsDefined(typeof(KnownRoles), role.Name))
            throw new RecordCannotBeDeletedException($"Predefined role cannot be deleted: Role name: '{role.Name}'");

        await _roleRepository.DeleteAsync(role);
        return Unit.Value;
    }
}