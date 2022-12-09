using MediatR;

namespace Boilerplate.Application.Commands;

public class CreateDummyCommandHandler : IRequestHandler<CreateDummyCommand, DummyDto>
{
    private readonly IGenericRepository<Dummy> _dummyRepository;
    private readonly IMapper _mapper;

    public CreateDummyCommandHandler(IGenericRepository<Dummy> dummyRepository, IMapper mapper)
    {
        _dummyRepository = dummyRepository ?? throw new ArgumentNullException(nameof(dummyRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<DummyDto> Handle(CreateDummyCommand request, CancellationToken cancellationToken)
    {
        var existingDummy = await _dummyRepository.GetAsync(s => s.Name == request.Name);

        if (existingDummy != null)
        {
            throw new DummyException($"There is a dummy. Name: '{request.Name}'");
        }

        var dummy = await _dummyRepository.AddAsync(_mapper.Map<Dummy>(request));

        return _mapper.Map<DummyDto>(dummy);
    }
}