using KeyManager.Application.Commands.Doors;
using KeyManager.Domain.Entities;

namespace KeyManager.Application.Tests.Commands.Doors;

public class CreateDoorCommandHandlerTests
{
    private readonly CreateDoorCommandHandler _doorHandler;
    private readonly Mock<IGenericRepository<Door>> _mockDoorRepository;

    public CreateDoorCommandHandlerTests()
    {
        _mockDoorRepository = new Mock<IGenericRepository<Door>>();
        var myProfile = new AutoMapperProfile();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        var mapper = new Mapper(configuration);
        _doorHandler = new CreateDoorCommandHandler(_mockDoorRepository.Object, mapper);
    }

    [Fact]
    public async Task Door_Create_WithGivenCreateDoorCommand_ShouldReturnCreateDoorDto()
    {
        //Arrange
        var mockDoorDto = new DoorDto
        {
            Id = 1,
            Name = "Test"
        };
        var mockDoor = new Door
        {
            Id = 1,
            Name = "Test"
        };
        _mockDoorRepository.Setup(s => s.AddAsync(It.IsAny<Door>())).ReturnsAsync(mockDoor);

        //Act
        var result = await _doorHandler.Handle(new CreateDoorCommand("Test"), default);

        //Assert
        Assert.Equal(result.Id, mockDoorDto.Id);
        Assert.Equal(result.Name, mockDoorDto.Name);

        _mockDoorRepository.VerifyAll();
    }

    [Fact]
    public async Task Door_PostAsync_WithGivenCreateDoorRequest_ShouldThrowExistingRecordException_IfRecordExists()
    {
        //Arrange
        var mockCreateDoorCommand = new CreateDoorCommand("Test");
        var mockDoor = new Door
        {
            Id = 1,
            Name = "Test"
        };
        _mockDoorRepository.Setup(s => s.GetAsync(p => p.Name == mockCreateDoorCommand.Name)).ReturnsAsync(mockDoor);

        //Act
        Task Result()
        {
            return _doorHandler.Handle(mockCreateDoorCommand, default);
        }

        //Assert
        var exception = await Assert.ThrowsAsync<DoorException>(Result);
        Assert.Equal("There is a door with the same name: 'Test'", exception.Message);
        _mockDoorRepository.VerifyAll();
    }
}