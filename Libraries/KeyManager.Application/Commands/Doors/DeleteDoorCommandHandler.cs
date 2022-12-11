namespace KeyManager.Application.Commands.Doors;

public class DeleteDoorCommandHandler : IRequestHandler<DeleteDoorCommand>
{
    private readonly IGenericRepository<Door> _doorRepository;

    public DeleteDoorCommandHandler(IGenericRepository<Door> doorRepository)
    {
        _doorRepository = doorRepository ?? throw new ArgumentNullException(nameof(doorRepository));
    }

    public async Task<Unit> Handle(DeleteDoorCommand request, CancellationToken cancellationToken)
    {
        var door = await _doorRepository.GetByIdAsync(request.Id);

        if (door == null) throw new DoorException($"Door not found while deleting. DoorId: '{request.Id}'");

        await _doorRepository.DeleteAsync(door);
        return Unit.Value;
    }
}