using System.Threading;
using KeyManager.Api.DTOs.Responses.Doors;
using KeyManager.Application.Commands.Doors;
using KeyManager.Application.Queries.Doors;
using MediatR;

namespace KeyManager.Api.Tests.Controllers;

public class DoorControllerTests
{
    private readonly Mock<ISender> _mockMediator;
    private readonly DoorController _sut;

    public DoorControllerTests()
    {
        _mockMediator = new Mock<ISender>();
        var myProfile = new AutoMapperProfile();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        var mapper = new Mapper(configuration);
        _sut = new DoorController(_mockMediator.Object, mapper);
    }

    [Fact]
    public async Task Door_GetAsync_ShouldReturnAllDoors()
    {
        //Arrange
        var mockDoorDto = new List<DoorDto>
        {
            new() { Id = 1, Name = "Test" },
            new() { Id = 2, Name = "Test2" }
        };
        var mockGetDoorsResponse = new List<GetDoorResponse>
        {
            new() { Id = 1, Name = "Test" },
            new() { Id = 2, Name = "Test2" }
        };
        _mockMediator.Setup(s => s.Send(It.IsAny<GetDoorsQuery>(), It.Is<CancellationToken>(x => x == default)))
            .ReturnsAsync(mockDoorDto);

        //Act
        var result = await _sut.GetAsync();

        //Assert
        var resultObject = (List<GetDoorResponse>)Assert.IsType<OkObjectResult>(result.Result).Value;
        Assert.NotNull(resultObject);
        Assert.Equal(mockGetDoorsResponse[0].Id, resultObject[0].Id);
        Assert.Equal(mockGetDoorsResponse[0].Name, resultObject[0].Name);
        Assert.Equal(mockGetDoorsResponse[1].Id, resultObject[1].Id);
        Assert.Equal(mockGetDoorsResponse[1].Name, resultObject[1].Name);
        _mockMediator.VerifyAll();
    }

    [Fact]
    public async Task Door_GetAsync_WithGivenId_ShouldReturnAllDoor()
    {
        //Arrange
        var mockDoorDto = new DoorDto
        {
            Id = 1,
            Name = "Test"
        };
        _mockMediator.Setup(s => s.Send(It.IsAny<GetDoorByIdQuery>()
            , It.Is<CancellationToken>(x => x == default))).ReturnsAsync(mockDoorDto);

        //Act
        var result = await _sut.GetAsync(1);

        //Assert
        var resultObject = (GetDoorResponse)Assert.IsType<OkObjectResult>(result.Result).Value;
        Assert.NotNull(resultObject);
        Assert.Equal(mockDoorDto.Id, resultObject.Id);
        Assert.Equal(mockDoorDto.Name, resultObject.Name);
        _mockMediator.VerifyAll();
    }

    [Fact]
    public async Task Door_PostAsync_WithGivenDoor_ShouldAddDoor()
    {
        //Arrange
        var mockCreateDoorCommand = new CreateDoorCommand("Test");
        var mockDoorDto = new DoorDto
        {
            Id = 1,
            Name = "Test"
        };
        _mockMediator.Setup(s => s.Send(It.IsAny<CreateDoorCommand>()
            , It.Is<CancellationToken>(x => x == default))).ReturnsAsync(mockDoorDto);

        //Act
        var result = await _sut.PostAsync(mockCreateDoorCommand);

        //Assert
        var resultObject = (CreateDoorResponse)Assert.IsType<CreatedResult>(result.Result).Value;
        Assert.NotNull(resultObject);
        Assert.Equal(mockDoorDto.Id, resultObject.Id);
        Assert.Equal(mockDoorDto.Name, resultObject.Name);
        _mockMediator.VerifyAll();
    }

    [Fact]
    public async Task Door_PutAsync_WithGivenDoor_ShouldUpdateDoor()
    {
        //Arrange
        var mockUpdateDoorCommand = new UpdateDoorCommand(1, "Test");
        var mockDoorDto = new DoorDto
        {
            Id = 1,
            Name = "Test2"
        };
        _mockMediator.Setup(s => s.Send(It.IsAny<UpdateDoorCommand>()
            , It.Is<CancellationToken>(x => x == default))).ReturnsAsync(mockDoorDto);

        //Act
        var result = await _sut.PutAsync(mockUpdateDoorCommand);

        //Assert
        var resultObject = (UpdateDoorResponse)Assert.IsType<OkObjectResult>(result.Result).Value;
        Assert.NotNull(resultObject);
        Assert.Equal(mockDoorDto.Id, resultObject.Id);
        Assert.Equal(mockDoorDto.Name, resultObject.Name);
        _mockMediator.VerifyAll();
    }

    [Fact]
    public async Task Door_DeleteAsync_WithGivenDoor_ShouldDeleteDoor()
    {
        //Arrange
        _mockMediator.Setup(s => s.Send(It.IsAny<DeleteDoorCommand>()
            , It.Is<CancellationToken>(x => x == default)));

        //Act
        await _sut.DeleteAsync(1);

        //Assert
        _mockMediator.VerifyAll();
    }
}