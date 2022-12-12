using Microsoft.EntityFrameworkCore;

namespace KeyManager.Application.Queries.Users;

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDto>
{
    private readonly DataContext _db;
    private readonly IMapper _mapper;

    public GetUserByIdQueryHandler(DataContext db, IMapper mapper)
    {
        _db = db ?? throw new ArgumentNullException(nameof(db));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<UserDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _db.Users.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (user == null) throw new RecordNotFoundException($"User not found. User ID: '{request.Id}'");
        return _mapper.Map<UserDto>(user);
    }
}