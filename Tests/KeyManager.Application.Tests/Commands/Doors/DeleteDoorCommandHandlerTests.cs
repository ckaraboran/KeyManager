using KeyManager.Application.Commands.Doors;
using KeyManager.Domain.Entities;

namespace KeyManager.Application.Tests.Commands.Doors;

public class DeleteDoorCommandHandlerTests
{
    private readonly DeleteDoorCommandHandler _doorHandler;
    private readonly Mock<IGenericRepository<Door>> _mockDoorRepository;

    public DeleteDoorCommandHandlerTests()
    {
        _mockDoorRepository = new Mock<IGenericRepository<Door>>();
        _doorHandler = new DeleteDoorCommandHandler(_mockDoorRepository.Object);
    }

    [Fact]
    public async Task Door_DeleteAsync_WithGivenId_ShouldBeVerified()
    {
        //Arrange
        var mockDoor = new Door
        {
            Id = 1,
            Name = "Test"
        };
        _mockDoorRepository.Setup(s => s.GetByIdAsync(It.IsAny<long>())).ReturnsAsync(mockDoor);
        _mockDoorRepository.Setup(s => s.DeleteAsync(It.IsAny<Door>()));

        //Act
        await _doorHandler.Handle(new DeleteDoorCommand(1), default);

        //Assert
        _mockDoorRepository.VerifyAll();
    }

    [Fact]
    public async Task Door_DeleteAsync_WithGivenId_ShouldThrowRecordNotFoundException_IfRecordDoesNotExist()
    {
        //Arrange
        _mockDoorRepository.Setup(s => s.GetByIdAsync(It.IsAny<long>())).ReturnsAsync((Door)null);

        //Act
        Task Result()
        {
            return _doorHandler.Handle(new DeleteDoorCommand(5), default);
        }

        //Act-Assert
        var exception = await Assert.ThrowsAsync<DoorException>(Result);
        Assert.Equal("Door not found while deleting. DoorId: '5'", exception.Message);
        _mockDoorRepository.VerifyAll();
    }
}