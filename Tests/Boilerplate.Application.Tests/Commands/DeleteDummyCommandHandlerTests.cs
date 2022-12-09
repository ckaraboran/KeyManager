using Boilerplate.Application.Commands;

namespace Boilerplate.Application.Tests.Commands;

public class DeleteDummyCommandHandlerTests
{
    private readonly DeleteDummyCommandHandler _dummyHandler;
    private readonly Mock<IGenericRepository<Domain.Entities.Dummy>> _mockDummyRepository;

    public DeleteDummyCommandHandlerTests()
    {
        _mockDummyRepository = new Mock<IGenericRepository<Domain.Entities.Dummy>>();
        _dummyHandler = new DeleteDummyCommandHandler(_mockDummyRepository.Object);
    }

    [Fact]
    public async Task Dummy_DeleteAsync_WithGivenId_ShouldBeVerified()
    {
        //Arrange
        var mockDummy = new Domain.Entities.Dummy
        {
            Id = 1,
            Name = "Test"
        };
        _mockDummyRepository.Setup(s => s.GetAsync(It.IsAny<int>())).ReturnsAsync(mockDummy);
        _mockDummyRepository.Setup(s => s.SoftDeleteAsync(It.IsAny<Domain.Entities.Dummy>()));

        //Act
        await _dummyHandler.Handle(new DeleteDummyCommand(1), default);

        //Assert
        _mockDummyRepository.VerifyAll();
    }

    [Fact]
    public async Task Dummy_DeleteAsync_WithGivenId_ShouldThrowRecordNotFoundException_IfRecordDoesNotExist()
    {
        //Arrange
        _mockDummyRepository.Setup(s => s.GetAsync(It.IsAny<int>())).ReturnsAsync((Domain.Entities.Dummy)null);

        //Act
        Task Result()
        {
            return _dummyHandler.Handle(new DeleteDummyCommand(5), default);
        }

        //Act-Assert
        var exception = await Assert.ThrowsAsync<DummyException>(Result);
        Assert.Equal("Dummy is not found while deleting. DummyId: '5'", exception.Message);
        _mockDummyRepository.VerifyAll();
    }
}