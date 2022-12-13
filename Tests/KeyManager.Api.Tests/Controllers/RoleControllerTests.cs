using System.Threading;
using KeyManager.Api.DTOs.Responses.Roles;
using KeyManager.Application.Commands.Roles;
using KeyManager.Application.Queries.Roles;
using MediatR;

namespace KeyManager.Api.Tests.Controllers;

public class RoleControllerTests
{
    private readonly Mock<ISender> _mockMediator;
    private readonly RoleController _sut;

    public RoleControllerTests()
    {
        _mockMediator = new Mock<ISender>();
        var myProfile = new AutoMapperProfile();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        var mapper = new Mapper(configuration);
        _sut = new RoleController(_mockMediator.Object, mapper);
    }

    [Fact]
    public async Task Given_GetRoleCommand_When_Get_Then_ReturnAllRoles()
    {
        //Arrange
        var mockRoleDto = new List<RoleDto>
        {
            new() { Id = 1, Name = "Test" },
            new() { Id = 2, Name = "Test2" }
        };
        var mockGetRolesResponse = new List<GetRoleResponse>
        {
            new() { Id = 1, Name = "Test" },
            new() { Id = 2, Name = "Test2" }
        };
        _mockMediator.Setup(s => s.Send(It.IsAny<GetRolesQuery>(), It.Is<CancellationToken>(x => x == default)))
            .ReturnsAsync(mockRoleDto);

        //Act
        var result = await _sut.GetAsync();

        //Assert
        var resultObject = (List<GetRoleResponse>)Assert.IsType<OkObjectResult>(result.Result).Value;
        Assert.NotNull(resultObject);
        Assert.Equal(mockGetRolesResponse[0].Id, resultObject[0].Id);
        Assert.Equal(mockGetRolesResponse[0].Name, resultObject[0].Name);
        Assert.Equal(mockGetRolesResponse[1].Id, resultObject[1].Id);
        Assert.Equal(mockGetRolesResponse[1].Name, resultObject[1].Name);
        _mockMediator.VerifyAll();
    }

    [Fact]
    public async Task Given_RoleGet_When_WithGivenId_Then_ReturnAllRole()
    {
        //Arrange
        var mockRoleDto = new RoleDto
        {
            Id = 1,
            Name = "Test"
        };
        _mockMediator.Setup(s => s.Send(It.IsAny<GetRoleByIdQuery>()
            , It.Is<CancellationToken>(x => x == default))).ReturnsAsync(mockRoleDto);

        //Act
        var result = await _sut.GetAsync(1);

        //Assert
        var resultObject = (GetRoleResponse)Assert.IsType<OkObjectResult>(result.Result).Value;
        Assert.NotNull(resultObject);
        Assert.Equal(mockRoleDto.Id, resultObject.Id);
        Assert.Equal(mockRoleDto.Name, resultObject.Name);
        _mockMediator.VerifyAll();
    }

    [Fact]
    public async Task Given_RolePost_When_WithGivenRole_Then_AddRole()
    {
        //Arrange
        var mockCreateRoleCommand = new CreateRoleCommand("Test");
        var mockRoleDto = new RoleDto
        {
            Id = 1,
            Name = "Test"
        };
        _mockMediator.Setup(s => s.Send(It.IsAny<CreateRoleCommand>()
            , It.Is<CancellationToken>(x => x == default))).ReturnsAsync(mockRoleDto);

        //Act
        var result = await _sut.PostAsync(mockCreateRoleCommand);

        //Assert
        var resultObject = (CreateRoleResponse)Assert.IsType<CreatedResult>(result.Result).Value;
        Assert.NotNull(resultObject);
        Assert.Equal(mockRoleDto.Id, resultObject.Id);
        Assert.Equal(mockRoleDto.Name, resultObject.Name);
        _mockMediator.VerifyAll();
    }

    [Fact]
    public async Task Given_RolePut_When_WithGivenRole_Then_UpdateRole()
    {
        //Arrange
        var mockUpdateRoleCommand = new UpdateRoleCommand(1, "Test");
        var mockRoleDto = new RoleDto
        {
            Id = 1,
            Name = "Test2"
        };
        _mockMediator.Setup(s => s.Send(It.IsAny<UpdateRoleCommand>()
            , It.Is<CancellationToken>(x => x == default))).ReturnsAsync(mockRoleDto);

        //Act
        var result = await _sut.PutAsync(mockUpdateRoleCommand);

        //Assert
        var resultObject = (UpdateRoleResponse)Assert.IsType<OkObjectResult>(result.Result).Value;
        Assert.NotNull(resultObject);
        Assert.Equal(mockRoleDto.Id, resultObject.Id);
        Assert.Equal(mockRoleDto.Name, resultObject.Name);
        _mockMediator.VerifyAll();
    }

    [Fact]
    public async Task Given_RoleDelete_When_WithGivenRole_Then_DeleteRole()
    {
        //Arrange
        _mockMediator.Setup(s => s.Send(It.IsAny<DeleteRoleCommand>()
            , It.Is<CancellationToken>(x => x == default)));

        //Act
        await _sut.DeleteAsync(1);

        //Assert
        _mockMediator.VerifyAll();
    }
}