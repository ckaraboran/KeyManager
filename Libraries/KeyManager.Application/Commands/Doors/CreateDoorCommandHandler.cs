namespace KeyManager.Application.Commands.Doors;

public class CreateDoorCommandHandler : IRequestHandler<CreateDoorCommand, DoorDto>
{
    private readonly IGenericRepository<Door> _doorRepository;
    private readonly IMapper _mapper;

    public CreateDoorCommandHandler(IGenericRepository<Door> doorRepository, IMapper mapper)
    {
        _doorRepository = doorRepository ?? throw new ArgumentNullException(nameof(doorRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<DoorDto> Handle(CreateDoorCommand request, CancellationToken cancellationToken)
    {
        var existingDoor = await _doorRepository.GetAsync(s => s.Name == request.Name);

        if (existingDoor != null)
            throw new RecordAlreadyExistsException($"There is a door with the same name: '{request.Name}'");

        var door = await _doorRepository.AddAsync(_mapper.Map<Door>(request));

        return _mapper.Map<DoorDto>(door);
    }
}