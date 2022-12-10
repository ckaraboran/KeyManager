using KeyManager.Application.Commands;
using KeyManager.Domain.Entities;

namespace KeyManager.Application.Tests.Commands;

public class DeleteDummyCommandHandlerTests
{
    private readonly DeleteDummyCommandHandler _dummyHandler;
    private readonly Mock<IGenericRepository<Dummy>> _mockDummyRepository;

    public DeleteDummyCommandHandlerTests()
    {
        _mockDummyRepository = new Mock<IGenericRepository<Dummy>>();
        _dummyHandler = new DeleteDummyCommandHandler(_mockDummyRepository.Object);
    }

    [Fact]
    public async Task Dummy_DeleteAsync_WithGivenId_ShouldBeVerified()
    {
        //Arrange
        var mockDummy = new Dummy
        {
            Id = 1,
            Name = "Test"
        };
        _mockDummyRepository.Setup(s => s.GetAsync(It.IsAny<long>())).ReturnsAsync(mockDummy);
        _mockDummyRepository.Setup(s => s.DeleteAsync(It.IsAny<Dummy>()));

        //Act
        await _dummyHandler.Handle(new DeleteDummyCommand(1), default);

        //Assert
        _mockDummyRepository.VerifyAll();
    }

    [Fact]
    public async Task Dummy_DeleteAsync_WithGivenId_ShouldThrowRecordNotFoundException_IfRecordDoesNotExist()
    {
        //Arrange
        _mockDummyRepository.Setup(s => s.GetAsync(It.IsAny<long>())).ReturnsAsync((Dummy)null);

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