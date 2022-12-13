using KeyManager.Application.Commands.Doors;
using KeyManager.Domain.Entities;

namespace KeyManager.Application.Tests.Commands.Doors;

public class UpdateDoorCommandHandlerTests
{
    private readonly UpdateDoorCommandHandler _doorHandler;
    private readonly Mock<IGenericRepository<Door>> _mockDoorRepository;

    public UpdateDoorCommandHandlerTests()
    {
        _mockDoorRepository = new Mock<IGenericRepository<Door>>();
        var myProfile = new AutoMapperProfile();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        var mapper = new Mapper(configuration);
        _doorHandler = new UpdateDoorCommandHandler(_mockDoorRepository.Object, mapper);
    }

    [Fact]
    public async Task Given_DoorPut_When_WithGivenUpdateDoorRequest_Then_ShouldReturnCreateDoorDto()
    {
        //Arrange
        var newDoor = new Door
        {
            Id = 1,
            Name = "New Door"
        };
        var mockDoorUpdateCommand = new UpdateDoorCommand(newDoor.Id, newDoor.Name);

        var oldDoor = new Door
        {
            Id = 1,
            Name = "Old Door"
        };

        _mockDoorRepository.Setup(s => s.GetByIdAsync(It.IsAny<long>())).ReturnsAsync(oldDoor);
        _mockDoorRepository.Setup(s => s.UpdateAsync(It.IsAny<Door>())).ReturnsAsync(newDoor);

        //Act
        var result = await _doorHandler.Handle(mockDoorUpdateCommand, default);

        //Assert
        Assert.Equal(result.Id, newDoor.Id);
        Assert.Equal(result.Name, newDoor.Name);
        _mockDoorRepository.VerifyAll();
    }

    [Fact]
    public async Task Given_DoorPut_When_RecordDoesNotExist_Then_ShouldThrowRecordNotFoundException_()
    {
        //Arrange
        var mockUpdateDoorCommand = new UpdateDoorCommand(1, "Test");
        _mockDoorRepository.Setup(s => s.GetByIdAsync(mockUpdateDoorCommand.Id))
            .ReturnsAsync((Door)null);

        //Act
        Task Result()
        {
            return _doorHandler.Handle(mockUpdateDoorCommand, default);
        }

        //Assert
        var exception = await Assert.ThrowsAsync<RecordNotFoundException>(Result);
        Assert.Equal("Door not found. DoorId: '1'", exception.Message);
        _mockDoorRepository.VerifyAll();
    }
}