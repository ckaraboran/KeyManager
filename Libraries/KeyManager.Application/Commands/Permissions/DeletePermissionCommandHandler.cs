namespace KeyManager.Application.Commands.Permissions;

public class DeletePermissionCommandHandler : IRequestHandler<DeletePermissionCommand>
{
    private readonly IGenericRepository<Permission> _permissionRepository;

    public DeletePermissionCommandHandler(IGenericRepository<Permission> permissionRepository)
    {
        _permissionRepository = permissionRepository ?? throw new ArgumentNullException(nameof(permissionRepository));
    }

    public async Task<Unit> Handle(DeletePermissionCommand request, CancellationToken cancellationToken)
    {
        var permission = await _permissionRepository.GetByIdAsync(request.Id);

        if (permission == null)
            throw new PermissionException($"Permission is not found while deleting. PermissionId: '{request.Id}'");

        await _permissionRepository.DeleteAsync(permission);
        return Unit.Value;
    }
}