using KeyManager.Api.DTOs.Requests;
using KeyManager.Application.Commands.Users;
using KeyManager.Application.Queries.Users;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace KeyManager.Api.Tests.Controllers;

public class LoginControllerTests
{
    private readonly Mock<IConfiguration> _mockConfiguration;
    private readonly Mock<ISender> _mockMediator;
    private readonly LoginController _sut;

    public LoginControllerTests()
    {
        _mockConfiguration = new Mock<IConfiguration>();
        _mockMediator = new Mock<ISender>();
        _sut = new LoginController(_mockConfiguration.Object, _mockMediator.Object);
    }

    [Fact]
    public async Task Given_User_When_In_AuthUsers_Then_ShouldGetToken()
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
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task Given_User_When_NotKnown_Then_ShouldReturnNotFound()
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
    public async Task Given_User_When_Not_In_AuthUsers_Then_Should_Return_NotFound()
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
}