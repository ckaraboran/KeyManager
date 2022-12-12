using System.Threading;
using KeyManager.Api.DTOs.Responses.Dummy;
using KeyManager.Application.Commands;
using KeyManager.Application.Queries;
using MediatR;

namespace KeyManager.Api.Tests.Controllers;

public class DummyControllerTests
{
    private readonly Mock<ISender> _mockMediator;
    private readonly DummyController _sut;

    public DummyControllerTests()
    {
        _mockMediator = new Mock<ISender>();
        var myProfile = new AutoMapperProfile();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        var mapper = new Mapper(configuration);
        _sut = new DummyController(_mockMediator.Object, mapper);
    }

    [Fact]
    public async Task Dummy_GetAsync_ShouldReturnAllDummies()
    {
        //Arrange
        var mockDummyDto = new List<DummyDto>
        {
            new() { Id = 1, Name = "Test" },
            new() { Id = 2, Name = "Test2" }
        };
        var mockGetDummiesResponse = new List<GetDummyResponse>
        {
            new() { Id = 1, Name = "Test" },
            new() { Id = 2, Name = "Test2" }
        };
        _mockMediator.Setup(s => s.Send(It.IsAny<GetAllDummiesQuery>(), It.Is<CancellationToken>(x => x == default)))
            .ReturnsAsync(mockDummyDto);

        //Act
        var result = await _sut.GetAsync();

        //Assert
        var resultObject = (List<GetDummyResponse>)Assert.IsType<OkObjectResult>(result.Result).Value;
        Assert.NotNull(resultObject);
        Assert.Equal(mockGetDummiesResponse[0].Id, resultObject[0].Id);
        Assert.Equal(mockGetDummiesResponse[0].Name, resultObject[0].Name);
        Assert.Equal(mockGetDummiesResponse[1].Id, resultObject[1].Id);
        Assert.Equal(mockGetDummiesResponse[1].Name, resultObject[1].Name);
        _mockMediator.VerifyAll();
    }

    [Fact]
    public async Task Dummy_GetAsync_WithGivenId_ShouldReturnAllDummy()
    {
        //Arrange
        var mockDummyDto = new DummyDto
        {
            Id = 1,
            Name = "Test"
        };
        _mockMediator.Setup(s => s.Send(It.IsAny<GetDummyQuery>()
            , It.Is<CancellationToken>(x => x == default))).ReturnsAsync(mockDummyDto);

        //Act
        var result = await _sut.GetAsync(1);

        //Assert
        var resultObject = (GetDummyResponse)Assert.IsType<OkObjectResult>(result.Result).Value;
        Assert.NotNull(resultObject);
        Assert.Equal(mockDummyDto.Id, resultObject.Id);
        Assert.Equal(mockDummyDto.Name, resultObject.Name);
        _mockMediator.VerifyAll();
    }

    [Fact]
    public async Task Dummy_PostAsync_WithGivenDummy_ShouldAddDummy()
    {
        //Arrange
        var mockCreateDummyCommand = new CreateDummyCommand("Test");
        var mockDummyDto = new DummyDto
        {
            Id = 1,
            Name = "Test"
        };
        _mockMediator.Setup(s => s.Send(It.IsAny<CreateDummyCommand>()
            , It.Is<CancellationToken>(x => x == default))).ReturnsAsync(mockDummyDto);

        //Act
        var result = await _sut.PostAsync(mockCreateDummyCommand);

        //Assert
        var resultObject = (CreateDummyResponse)Assert.IsType<CreatedResult>(result.Result).Value;
        Assert.NotNull(resultObject);
        Assert.Equal(mockDummyDto.Id, resultObject.Id);
        Assert.Equal(mockDummyDto.Name, resultObject.Name);
        _mockMediator.VerifyAll();
    }

    [Fact]
    public async Task Dummy_PutAsync_WithGivenDummy_ShouldUpdateDummy()
    {
        //Arrange
        var mockUpdateDummyCommand = new UpdateDummyCommand(1, "Test");
        var mockDummyDto = new DummyDto
        {
            Id = 1,
            Name = "Test2"
        };
        _mockMediator.Setup(s => s.Send(It.IsAny<UpdateDummyCommand>()
            , It.Is<CancellationToken>(x => x == default))).ReturnsAsync(mockDummyDto);

        //Act
        var result = await _sut.PutAsync(mockUpdateDummyCommand);

        //Assert
        var resultObject = (UpdateDummyResponse)Assert.IsType<OkObjectResult>(result.Result).Value;
        Assert.NotNull(resultObject);
        Assert.Equal(mockDummyDto.Id, resultObject.Id);
        Assert.Equal(mockDummyDto.Name, resultObject.Name);
        _mockMediator.VerifyAll();
    }

    [Fact]
    public async Task Dummy_DeleteAsync_WithGivenDummy_ShouldDeleteDummy()
    {
        //Arrange
        _mockMediator.Setup(s => s.Send(It.IsAny<DeleteDummyCommand>()
            , It.Is<CancellationToken>(x => x == default)));

        //Act
        await _sut.DeleteAsync(1);

        //Assert
        _mockMediator.VerifyAll();
    }
}