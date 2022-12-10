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
    public async Task Door_PutAsync_WithGivenUpdateDoorRequest_ShouldReturnCreateDoorDto()
    {
        var doorName = "Test";
        //Arrange
        var mockDoorUpdateCommand = new UpdateDoorCommand(1, doorName);

        var mockDoorDto = new DoorDto
        {
            Id = 1,
            Name = doorName
        };
        var mockDoor = new Door
        {
            Id = 1,
            Name = doorName
        };
        _mockDoorRepository.Setup(s => s.GetByIdAsync(It.IsAny<long>())).ReturnsAsync(mockDoor);
        _mockDoorRepository.Setup(s => s.UpdateAsync(It.IsAny<Door>())).ReturnsAsync(mockDoor);

        //Act
        var result = await _doorHandler.Handle(mockDoorUpdateCommand, default);

        //Assert
        Assert.Equal(result.Id, mockDoorDto.Id);
        Assert.Equal(result.Name, mockDoorDto.Name);
        _mockDoorRepository.VerifyAll();
    }

    [Fact]
    public async Task
        Door_PutAsync_WithGivenUpdateDoorRequest_ShouldThrowRecordNotFoundException_IfRecordDoesNotExist()
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
        var exception = await Assert.ThrowsAsync<DoorException>(Result);
        Assert.Equal("Door is not found. DoorId: '1'", exception.Message);
        _mockDoorRepository.VerifyAll();
    }
}