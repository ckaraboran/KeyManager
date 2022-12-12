using System.Threading;
using KeyManager.Api.DTOs.Responses.UserRoles;
using KeyManager.Application.Commands.UserRoles;
using KeyManager.Application.Queries.UserRoles;
using MediatR;

namespace KeyManager.Api.Tests.Controllers;

public class UserRoleControllerTests
{
    private readonly Mock<ISender> _mockMediator;
    private readonly UserRoleController _sut;

    public UserRoleControllerTests()
    {
        _mockMediator = new Mock<ISender>();
        var myProfile = new AutoMapperProfile();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        var mapper = new Mapper(configuration);
        _sut = new UserRoleController(_mockMediator.Object, mapper);
    }

    [Fact]
    public async Task UserRole_GetAsync_ShouldReturnAllUserRoles()
    {
        //Arrange
        var mockUserRoleDto = new List<UserRoleWithNamesDto>
        {
            new() { Id = 1, UserId = 1, UserName = "Username 1", RoleId = 1, RoleName = "Role name 1" },
            new() { Id = 1, UserId = 2, UserName = "Username 2", RoleId = 2, RoleName = "Role name 2" }
        };
        _mockMediator.Setup(s => s.Send(It.IsAny<GetUserRolesQuery>(), It.Is<CancellationToken>(x => x == default)))
            .ReturnsAsync(mockUserRoleDto);

        //Act
        var result = await _sut.GetAsync();

        //Assert
        var resultObject = (List<GetUserRoleResponse>)Assert.IsType<OkObjectResult>(result.Result).Value;
        Assert.NotNull(resultObject);
        Assert.Equal(mockUserRoleDto[0].Id, resultObject[0].Id);
        Assert.Equal(mockUserRoleDto[0].UserId, resultObject[0].UserId);
        Assert.Equal(mockUserRoleDto[0].UserName, resultObject[0].UserName);
        Assert.Equal(mockUserRoleDto[0].RoleId, resultObject[0].RoleId);
        Assert.Equal(mockUserRoleDto[0].RoleName, resultObject[0].RoleName);
        Assert.Equal(mockUserRoleDto[1].Id, resultObject[1].Id);
        Assert.Equal(mockUserRoleDto[1].UserId, resultObject[1].UserId);
        Assert.Equal(mockUserRoleDto[1].UserName, resultObject[1].UserName);
        Assert.Equal(mockUserRoleDto[1].RoleId, resultObject[1].RoleId);
        Assert.Equal(mockUserRoleDto[1].RoleName, resultObject[1].RoleName);
        _mockMediator.VerifyAll();
    }

    [Fact]
    public async Task UserRole_PostAsync_WithGivenUserRole_ShouldAddUserRole()
    {
        //Arrange
        var mockUserRoleDto = new UserRoleDto
        {
            Id = 1, UserId = 1, RoleId = 1
        };
        _mockMediator.Setup(s => s.Send(It.IsAny<CreateUserRoleCommand>()
            , It.Is<CancellationToken>(x => x == default))).ReturnsAsync(mockUserRoleDto);

        //Act
        var result =
            await _sut.PostAsync(new CreateUserRoleCommand(mockUserRoleDto.UserId, mockUserRoleDto.RoleId));

        //Assert
        var resultObject = (CreateUserRoleResponse)Assert.IsType<CreatedResult>(result.Result).Value;
        Assert.NotNull(resultObject);
        Assert.Equal(mockUserRoleDto.Id, resultObject.Id);
        Assert.Equal(mockUserRoleDto.UserId, resultObject.UserId);
        Assert.Equal(mockUserRoleDto.RoleId, resultObject.RoleId);
        _mockMediator.VerifyAll();
    }


    [Fact]
    public async Task UserRole_DeleteAsync_WithGivenUserRole_ShouldDeleteUserRole()
    {
        //Arrange
        _mockMediator.Setup(s => s.Send(It.IsAny<DeleteUserRoleCommand>()
            , It.Is<CancellationToken>(x => x == default)));

        //Act
        await _sut.DeleteAsync(1);

        //Assert
        _mockMediator.VerifyAll();
    }
}