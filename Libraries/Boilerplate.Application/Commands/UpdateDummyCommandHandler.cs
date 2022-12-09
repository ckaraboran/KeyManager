using MediatR;

namespace Boilerplate.Application.Commands;

public class UpdateDummyCommandHandler : IRequestHandler<UpdateDummyCommand, DummyDto>
{
    private readonly IGenericRepository<Dummy> _dummyRepository;
    private readonly IMapper _mapper;

    public UpdateDummyCommandHandler(IGenericRepository<Dummy> dummyRepository, IMapper mapper)
    {
        _dummyRepository = dummyRepository ?? throw new ArgumentNullException(nameof(dummyRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<DummyDto> Handle(UpdateDummyCommand request, CancellationToken cancellationToken)
    {
        var existingDummy = await _dummyRepository.GetAsync(request.Id);

        if (existingDummy == null)
            throw new DummyException($"Dummy is not found while updating. DummyId: '{request.Id}'");

        existingDummy = _mapper.Map<Domain.Entities.Dummy>(request);
        var updatedDummy = await _dummyRepository.UpdateAsync(existingDummy);

        return _mapper.Map<DummyDto>(updatedDummy);
    }
}