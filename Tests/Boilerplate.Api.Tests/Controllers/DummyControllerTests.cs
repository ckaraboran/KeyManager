using System.Threading;
using Boilerplate.Api.DTOs.Responses.Dummy;
using Boilerplate.Application.Commands;
using Boilerplate.Application.Queries;
using MediatR;

namespace Boilerplate.Api.Tests.Controllers;

public class DummyControllerTests
{
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ISender> _mockMediator;
    private readonly DummyController _sut;

    public DummyControllerTests()
    {
        _mockMediator = new Mock<ISender>();
        _mockMapper = new Mock<IMapper>();
        _sut = new DummyController(_mockMediator.Object, _mockMapper.Object);
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
        _mockMapper.Setup(m => m.Map<List<GetDummyResponse>>(mockDummyDto)).Returns(mockGetDummiesResponse);

        //Act
        var result = await _sut.GetAsync();

        //Assert
        var resultObject = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(mockGetDummiesResponse, resultObject!.Value);
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
        var mockGetDummyResponse = new GetDummyResponse
        {
            Id = 1,
            Name = "Test"
        };
        _mockMediator.Setup(s => s.Send(It.IsAny<GetDummyQuery>()
            , It.Is<CancellationToken>(x => x == default))).ReturnsAsync(mockDummyDto);
        _mockMapper.Setup(m => m.Map<GetDummyResponse>(mockDummyDto)).Returns(mockGetDummyResponse);

        //Act
        var result = await _sut.GetAsync(1);

        //Assert
        var resultObject = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(mockGetDummyResponse, resultObject!.Value);
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
        var mockCreateDummyResponse = new CreateDummyResponse
        {
            Id = 1,
            Name = "Test"
        };
        _mockMediator.Setup(s => s.Send(It.IsAny<CreateDummyCommand>()
            , It.Is<CancellationToken>(x => x == default))).ReturnsAsync(mockDummyDto);
        _mockMapper.Setup(m => m.Map<DummyDto>(mockCreateDummyCommand)).Returns(mockDummyDto);
        _mockMapper.Setup(m => m.Map<CreateDummyResponse>(mockDummyDto)).Returns(mockCreateDummyResponse);

        //Act
        var result = await _sut.PostAsync(mockCreateDummyCommand);

        //Assert
        var resultObject = Assert.IsType<CreatedResult>(result.Result);
        Assert.Equal(mockCreateDummyResponse, resultObject!.Value);
        _mockMediator.VerifyAll();
    }

    [Fact]
    public async Task Dummy_PutAsync_WithGivenDummy_ShouldUpdateDummy()
    {
        //Arrange
        var mockUpdateDummyCommand = new UpdateDummyCommand(1, "Test");
        var mockUpdateDummyResponse = new UpdateDummyResponse
        {
            Id = 1,
            Name = "Test2"
        };
        var mockDummyDto = new DummyDto
        {
            Id = 1,
            Name = "Test2"
        };
        _mockMediator.Setup(s => s.Send(It.IsAny<UpdateDummyCommand>()
            , It.Is<CancellationToken>(x => x == default))).ReturnsAsync(mockDummyDto);
        _mockMapper.Setup(m => m.Map<DummyDto>(mockUpdateDummyCommand)).Returns(mockDummyDto);
        _mockMapper.Setup(m => m.Map<UpdateDummyResponse>(mockDummyDto)).Returns(mockUpdateDummyResponse);

        //Act
        var result = await _sut.PutAsync(mockUpdateDummyCommand);

        //Assert
        var resultObject = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(mockUpdateDummyResponse, resultObject!.Value);
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