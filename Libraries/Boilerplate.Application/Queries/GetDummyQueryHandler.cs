using Boilerplate.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Boilerplate.Application.Queries;

public class GetDummyQueryHandler : IRequestHandler<GetDummyQuery, DummyDto>
{
    private readonly DataContext _db;
    private readonly IMapper _mapper;

    public GetDummyQueryHandler(DataContext db, IMapper mapper)
    {
        _db = db ?? throw new ArgumentNullException(nameof(db));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<DummyDto> Handle(GetDummyQuery request, CancellationToken cancellationToken)
    {
        var dummy = await _db.Dummies.AsNoTracking().FirstOrDefaultAsync(x=>x.Id == request.Id, cancellationToken);
        return _mapper.Map<DummyDto>(dummy);
    }
}