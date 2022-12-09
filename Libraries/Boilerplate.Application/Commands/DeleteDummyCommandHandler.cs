using MediatR;

namespace Boilerplate.Application.Commands;

public class DeleteDummyCommandHandler : IRequestHandler<DeleteDummyCommand>
{
    private readonly IGenericRepository<Dummy> _dummyRepository;

    public DeleteDummyCommandHandler(IGenericRepository<Dummy> dummyRepository)
    {
        _dummyRepository = dummyRepository ?? throw new ArgumentNullException(nameof(dummyRepository));
    }

    public async Task<Unit> Handle(DeleteDummyCommand request, CancellationToken cancellationToken)
    {
        var dummy = await _dummyRepository.GetAsync(request.Id);

        if (dummy == null) throw new DummyException($"Dummy is not found while deleting. DummyId: '{request.Id}'");

        await _dummyRepository.SoftDeleteAsync(dummy);
        return Unit.Value;
    }
}