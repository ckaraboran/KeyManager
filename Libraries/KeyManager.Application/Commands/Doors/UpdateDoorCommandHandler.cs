namespace KeyManager.Application.Commands.Doors;

public class UpdateDoorCommandHandler : IRequestHandler<UpdateDoorCommand, DoorDto>
{
    private readonly IGenericRepository<Door> _doorRepository;
    private readonly IMapper _mapper;

    public UpdateDoorCommandHandler(IGenericRepository<Door> doorRepository, IMapper mapper)
    {
        _doorRepository = doorRepository ?? throw new ArgumentNullException(nameof(doorRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<DoorDto> Handle(UpdateDoorCommand request, CancellationToken cancellationToken)
    {
        var existingDoor = await _doorRepository.GetByIdAsync(request.Id);

        if (existingDoor == null)
            throw new DoorException($"Door already exists. DoorId: '{request.Id}'");

        existingDoor = _mapper.Map<Door>(request);
        var updatedDoor = await _doorRepository.UpdateAsync(existingDoor);

        return _mapper.Map<DoorDto>(updatedDoor);
    }
}