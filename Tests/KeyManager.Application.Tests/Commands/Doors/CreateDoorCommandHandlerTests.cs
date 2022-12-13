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
    public async Task Given_DoorCreate_When_WithGivenCreateDoorCommand_Then_ShouldReturnCreateDoorDto()
    {
        //Arrange
        var mockDoor = new Door
        {
            Id = 1,
            Name = "Test"
        };
        _mockDoorRepository.Setup(s => s.AddAsync(It.IsAny<Door>())).ReturnsAsync(mockDoor);

        //Act
        var result = await _doorHandler.Handle(new CreateDoorCommand(mockDoor.Name), default);

        //Assert
        Assert.Equal(result.Id, mockDoor.Id);
        Assert.Equal(result.Name, mockDoor.Name);

        _mockDoorRepository.VerifyAll();
    }

    [Fact]
    public async Task
        Given_DoorPost_When_WithGivenCreateDoorRequest_Then_ShouldThrowExistingRecordException_IfRecordExists()
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
        var exception = await Assert.ThrowsAsync<RecordAlreadyExistsException>(Result);
        Assert.Equal("There is a door with the same name: 'Test'", exception.Message);
        _mockDoorRepository.VerifyAll();
    }
}