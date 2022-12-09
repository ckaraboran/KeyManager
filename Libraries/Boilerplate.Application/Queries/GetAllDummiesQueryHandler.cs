using Boilerplate.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Boilerplate.Application.Queries;

public class GetAllDummiesQueryHandler : IRequestHandler<GetAllDummiesQuery, List<DummyDto>>
{
    private readonly DataContext _db;
    private readonly IMapper _mapper;

    public GetAllDummiesQueryHandler(DataContext db, IMapper mapper)
    {
        _db = db ?? throw new ArgumentNullException(nameof(db));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<List<DummyDto>> Handle(GetAllDummiesQuery request, CancellationToken cancellationToken)
    {
        var dummies = await _db.Dummies.AsNoTracking().ToListAsync(cancellationToken);
        return _mapper.Map<List<DummyDto>>(dummies);
    }
}