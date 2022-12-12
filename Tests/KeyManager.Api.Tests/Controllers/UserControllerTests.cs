using System.Threading;
using KeyManager.Api.DTOs.Requests;
using KeyManager.Api.DTOs.Responses.Users;
using KeyManager.Application.Commands.Users;
using KeyManager.Application.Queries.Users;
using MediatR;

namespace KeyManager.Api.Tests.Controllers;

public class UserControllerTests
{
    private readonly Mock<ISender> _mockMediator;
    private readonly UserController _sut;

    public UserControllerTests()
    {
        _mockMediator = new Mock<ISender>();
        var myProfile = new AutoMapperProfile();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        var mapper = new Mapper(configuration);
        _sut = new UserController(_mockMediator.Object, mapper);
    }

    [Fact]
    public async Task User_GetAsync_ShouldReturnAllUsers()
    {
        //Arrange
        var mockUserDto = new List<UserDto>
        {
            new() { Id = 1, Name = "Test" },
            new() { Id = 2, Name = "Test2" }
        };
        var mockGetUsersResponse = new List<GetUserResponse>
        {
            new() { Id = 1, Name = "Test" },
            new() { Id = 2, Name = "Test2" }
        };
        _mockMediator.Setup(s => s.Send(It.IsAny<GetUsersQuery>(), It.Is<CancellationToken>(x => x == default)))
            .ReturnsAsync(mockUserDto);

        //Act
        var result = await _sut.GetAsync();

        //Assert
        var resultObject = (List<GetUserResponse>)Assert.IsType<OkObjectResult>(result.Result).Value;
        Assert.NotNull(resultObject);
        Assert.Equal(mockGetUsersResponse[0].Id, resultObject[0].Id);
        Assert.Equal(mockGetUsersResponse[0].Name, resultObject[0].Name);
        Assert.Equal(mockGetUsersResponse[1].Id, resultObject[1].Id);
        Assert.Equal(mockGetUsersResponse[1].Name, resultObject[1].Name);

        _mockMediator.VerifyAll();
    }

    [Fact]
    public async Task User_GetAsync_WithGivenId_ShouldReturnAllUser()
    {
        //Arrange
        var mockUserDto = new UserDto
        {
            Id = 1,
            Name = "Test"
        };
        _mockMediator.Setup(s => s.Send(It.IsAny<GetUserByIdQuery>()
            , It.Is<CancellationToken>(x => x == default))).ReturnsAsync(mockUserDto);

        //Act
        var result = await _sut.GetAsync(1);

        //Assert
        var resultObject = (GetUserResponse)Assert.IsType<OkObjectResult>(result.Result).Value;
        Assert.NotNull(resultObject);
        Assert.Equal(mockUserDto.Id, resultObject.Id);
        Assert.Equal(mockUserDto.Name, resultObject.Name);
        _mockMediator.VerifyAll();
    }

    [Fact]
    public async Task User_PostAsync_WithGivenUser_ShouldAddUser()
    {
        //Arrange
        var mockCreateUserCommand =
            new CreateUserCommand("Test Username", "Test name", "Test surname", "Test password");
        var mockUserDto = new UserDto
        {
            Id = 1,
            Name = "Test"
        };
        _mockMediator.Setup(s => s.Send(It.IsAny<CreateUserCommand>()
            , It.Is<CancellationToken>(x => x == default))).ReturnsAsync(mockUserDto);

        //Act
        var result = await _sut.PostAsync(mockCreateUserCommand);

        //Assert
        var resultObject = (CreateUserResponse)Assert.IsType<CreatedResult>(result.Result).Value;
        Assert.NotNull(resultObject);
        Assert.Equal(mockUserDto.Id, resultObject.Id);
        Assert.Equal(mockUserDto.Name, resultObject.Name);
        _mockMediator.VerifyAll();
    }

    [Fact]
    public async Task User_PutAsync_WithGivenUser_ShouldUpdateUser()
    {
        //Arrange
        var mockUpdateUserCommand =
            new UpdateUserCommand(1, "Test name", "Test surname");
        var mockUserDto = new UserDto
        {
            Id = 1,
            Name = "Test2"
        };
        _mockMediator.Setup(s => s.Send(It.IsAny<UpdateUserCommand>()
            , It.Is<CancellationToken>(x => x == default))).ReturnsAsync(mockUserDto);

        //Act
        var result = await _sut.PutAsync(mockUpdateUserCommand);

        //Assert
        var resultObject = (UpdateUserResponse)Assert.IsType<OkObjectResult>(result.Result).Value;
        Assert.NotNull(resultObject);
        Assert.Equal(mockUserDto.Id, resultObject.Id);
        Assert.Equal(mockUserDto.Name, resultObject.Name);
        _mockMediator.VerifyAll();
    }

    [Fact]
    public async Task User_DeleteAsync_WithGivenUser_ShouldDeleteUser()
    {
        //Arrange
        _mockMediator.Setup(s => s.Send(It.IsAny<DeleteUserCommand>()
            , It.Is<CancellationToken>(x => x == default)));

        //Act
        await _sut.DeleteAsync(1);

        //Assert
        _mockMediator.VerifyAll();
    }

    [Fact]
    public async Task Given_UpdatePasswordRequest_When_KnownUser_ThenShouldCallMediator()
    {
        //Arrange
        var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Name, "Test name")
        }, "mock"));
        _sut.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };
        _mockMediator.Setup(s => s.Send(It.IsAny<UpdateUserPasswordCommand>()
            , It.Is<CancellationToken>(x => x == default)));

        //Act
        await _sut.UpdatePasswordAsync(new UpdateUserPasswordRequest("old", "new"));

        //Assert
        _mockMediator.VerifyAll();
    }

    [Fact]
    public async Task Given_UpdatePasswordRequest_When_UserNotKnown_ThenShouldReturnUnAuthorized()
    {
        //Arrange
        var user = new ClaimsPrincipal(new ClaimsIdentity(Array.Empty<Claim>(), "mock"));
        _sut.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };
        _mockMediator.Setup(s => s.Send(It.IsAny<UpdateUserPasswordCommand>()
            , It.Is<CancellationToken>(x => x == default)));

        //Act
        var result = await _sut.UpdatePasswordAsync(new UpdateUserPasswordRequest("old", "new"));

        //Assert
        Assert.IsType<UnauthorizedResult>(result);
        _mockMediator.Verify(s => s.Send(It.IsAny<UpdateUserPasswordCommand>()
            , It.Is<CancellationToken>(x => x == default)), Times.Never);
    }

    [Fact]
    public void Given_UserExist_When_OpenDoor_Then_ShouldAddDoor()
    {
        //Arrange
        var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Role, "Test name"),
            new Claim(ClaimTypes.Role, "Admin1"),
            new Claim(ClaimTypes.Role, "User1")
        }, "mock"));
        _sut.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };

        //Act
        var result = _sut.AdminEndPoint();

        //Assert
        var resultValue = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Hi. you have these roles: Test name, Admin1, User1", resultValue.Value);
    }
}