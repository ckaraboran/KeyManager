using Microsoft.EntityFrameworkCore;

namespace KeyManager.Application.Queries.Incidents;

public class GetIncidentsQueryHandler : IRequestHandler<GetIncidentsQuery, List<IncidentWithNamesDto>>
{
    private readonly DataContext _db;

    public GetIncidentsQueryHandler(DataContext db)
    {
        _db = db ?? throw new ArgumentNullException(nameof(db));
    }

    public async Task<List<IncidentWithNamesDto>> Handle(GetIncidentsQuery request,
        CancellationToken cancellationToken)
    {
        var query = from incident in _db.Incidents
            join user in _db.Users on incident.UserId equals user.Id
            join door in _db.Doors on incident.DoorId equals door.Id
            select new IncidentWithNamesDto
            {
                Id = incident.Id,
                DoorId = door.Id,
                DoorName = door.Name,
                UserId = user.Id,
                UserName = user.Name,
                IncidentDate = incident.IncidentDate
            };
        return await query.AsNoTracking().ToListAsync(cancellationToken);
    }
}