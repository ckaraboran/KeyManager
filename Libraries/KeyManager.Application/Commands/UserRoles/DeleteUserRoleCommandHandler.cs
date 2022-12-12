namespace KeyManager.Application.Commands.UserRoles;

public class DeleteUserRoleCommandHandler : IRequestHandler<DeleteUserRoleCommand>
{
    private readonly IGenericRepository<UserRole> _userRoleRepository;

    public DeleteUserRoleCommandHandler(IGenericRepository<UserRole> userRoleRepository)
    {
        _userRoleRepository = userRoleRepository ?? throw new ArgumentNullException(nameof(userRoleRepository));
    }

    public async Task<Unit> Handle(DeleteUserRoleCommand request, CancellationToken cancellationToken)
    {
        var userRole = await _userRoleRepository.GetByIdAsync(request.Id);

        if (userRole == null)
            throw new RecordNotFoundException($"UserRole not found while deleting. UserRoleId: '{request.Id}'");

        await _userRoleRepository.DeleteAsync(userRole);
        return Unit.Value;
    }
}