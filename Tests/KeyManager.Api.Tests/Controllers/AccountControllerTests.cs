using System.Threading;
using KeyManager.Api.DTOs.Requests;
using KeyManager.Api.DTOs.Responses.Users;
using KeyManager.Application.Commands.Users;
using KeyManager.Application.Queries.Users;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace KeyManager.Api.Tests.Controllers;

public class AccountControllerTests
{
    private readonly Mock<IConfiguration> _mockConfiguration;
    private readonly Mock<ISender> _mockMediator;
    private readonly AccountController _sut;

    public AccountControllerTests()
    {
        _mockConfiguration = new Mock<IConfiguration>();
        _mockMediator = new Mock<ISender>();
        _sut = new AccountController(_mockConfiguration.Object, _mockMediator.Object);
    }

    [Fact]
    public async Task Given_User_When_In_AuthUsers_Then_GetToken()
    {
        //Arrange
        _mockMediator.Setup(s => s.Send(It.IsAny<CheckAuthenticationCommand>(), default))
            .ReturnsAsync(true);
        _mockMediator.Setup(s => s.Send(It.IsAny<GetUserRolesByUsername>(), default))
            .ReturnsAsync(new UserWithRolesDto { Username = "Test", RoleNames = new List<string> { "Admin" } });
        _mockConfiguration.Setup(s => s["Jwt:Key"]).Returns("8B672zVGcqNxuPEHX9dY");
        _mockConfiguration.Setup(s => s["Jwt:Issuer"]).Returns("Test");
        _mockConfiguration.Setup(s => s["Jwt:Audience"]).Returns("Test");

        //Act
        var result = await _sut.Login(new UserLoginRequest());
        var resultObject = (UserTokenResponse)Assert.IsType<OkObjectResult>(result).Value;
        Assert.NotNull(resultObject);
        Assert.NotEmpty(resultObject.Token);
    }

    [Fact]
    public async Task Given_User_When_NotKnown_Then_ReturnNotFound()
    {
        //Arrange
        _mockMediator.Setup(s => s.Send(It.IsAny<CheckAuthenticationCommand>(), default))
            .ReturnsAsync(false);
        _mockMediator.Setup(s => s.Send(It.IsAny<GetUserRolesByUsername>(), default))
            .ReturnsAsync(new UserWithRolesDto { Username = "Test", RoleNames = new List<string> { "Admin" } });
        _mockConfiguration.Setup(s => s["Jwt:Key"]).Returns("8B672zVGcqNxuPEHX9dY");
        _mockConfiguration.Setup(s => s["Jwt:Issuer"]).Returns("Test");
        _mockConfiguration.Setup(s => s["Jwt:Audience"]).Returns("Test");

        //Act
        var result = await _sut.Login(new UserLoginRequest());
        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task Given_User_When_Not_In_AuthUsers_Then_Return_NotFound()
    {
        //Arrange
        _mockMediator.Setup(s => s.Send(It.IsAny<GetUserRolesByUsername>(), default))
            .ReturnsAsync((UserWithRolesDto)null);
        _mockConfiguration.Setup(s => s["Jwt:Key"]).Returns("8B672zVGcqNxuPEHX9dY");
        _mockConfiguration.Setup(s => s["Jwt:Issuer"]).Returns("Test");
        _mockConfiguration.Setup(s => s["Jwt:Audience"]).Returns("Test");

        //Act
        var result = await _sut.Login(new UserLoginRequest());
        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task Given_UpdatePasswordRequest_When_UserNotKnown_Then_ReturnUnAuthorized()
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
    public async Task Given_UpdatePasswordRequest_When_KnownUser_Then_CallMediator()
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
    public void Given_AuthenticatedUser_When_AskRoles_ThenGetRoles()
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