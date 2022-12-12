namespace KeyManager.Application.Commands.Doors;

public class OpenDoorCommandHandler : IRequestHandler<OpenDoorCommand, bool>
{
    private readonly IGenericRepository<Door> _doorRepository;
    private readonly IGenericRepository<Incident> _incidentRepository;
    private readonly IGenericRepository<Permission> _permissionRepository;
    private readonly IGenericRepository<User> _userRepository;


    public OpenDoorCommandHandler(IGenericRepository<Door> doorRepository,
        IGenericRepository<User> userRepository, IGenericRepository<Permission> permissionRepository,
        IGenericRepository<Incident> incidentRepository)
    {
        _doorRepository = doorRepository ?? throw new ArgumentNullException(nameof(doorRepository));
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _permissionRepository = permissionRepository ?? throw new ArgumentNullException(nameof(permissionRepository));
        _incidentRepository = incidentRepository ?? throw new ArgumentNullException(nameof(incidentRepository));
    }

    public async Task<bool> Handle(OpenDoorCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetAsync(x => x.Username == request.Username);
        if (user == null) throw new DoorException($"User not found. Username: '{request.Username}'");

        var door = await _doorRepository.GetByIdAsync(request.DoorId);
        if (door == null) throw new DoorException($"Door not found. Door ID: '{request.DoorId}'");

        var permission = await _permissionRepository.GetAsync(p =>
            p.DoorId == door.Id && p.UserId == user.Id);

        if (permission == null) return false;

        var incident = new Incident
        {
            DoorId = door.Id,
            UserId = user.Id,
            IncidentDate = DateTimeOffset.UtcNow
        };

        await _incidentRepository.AddAsync(incident);
        return true;
    }
}