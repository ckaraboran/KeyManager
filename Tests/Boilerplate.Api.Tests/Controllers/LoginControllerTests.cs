using Boilerplate.Domain.Interfaces;
using Boilerplate.Domain.Models;
using Microsoft.Extensions.Configuration;

namespace Boilerplate.Api.Tests.Controllers;

public class LoginControllerTests
{
    private readonly Mock<IConfiguration> _mockConfiguration;
    private readonly Mock<IAuthUsersRepository> _mockUserRepository;
    private readonly LoginController _sut;
    
    public LoginControllerTests()
    {
        _mockConfiguration = new Mock<IConfiguration>();
        _mockUserRepository = new Mock<IAuthUsersRepository>();
        _sut = new LoginController(_mockConfiguration.Object, _mockUserRepository.Object);
    }

    [Fact]
    public async Task Given_User_When_In_AuthUsers_Then_ShouldGetToken()
    {
        //Arrange
        _mockUserRepository.Setup(s => s.GetUserAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(new UserModel(){Username = "Test", Password = "Test", Role = "Test"});
        _mockConfiguration.Setup(s=>s["Jwt:Key"]).Returns("8B672zVGcqNxuPEHX9dY");
        _mockConfiguration.Setup(s=>s["Jwt:Issuer"]).Returns("Test");
        _mockConfiguration.Setup(s=>s["Jwt:Audience"]).Returns("Test");
        
        //Act
        var result = await _sut.Login(new UserLogin());
        Assert.IsType<OkObjectResult>(result);
    }
    
    [Fact]
    public async Task Given_User_When_Not_In_AuthUsers_Then_Should_Return_NotFound()
    {
        //Arrange
        _mockUserRepository.Setup(s => s.GetUserAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync((UserModel)null);
        _mockConfiguration.Setup(s=>s["Jwt:Key"]).Returns("8B672zVGcqNxuPEHX9dY");
        _mockConfiguration.Setup(s=>s["Jwt:Issuer"]).Returns("Test");
        _mockConfiguration.Setup(s=>s["Jwt:Audience"]).Returns("Test");
        
        //Act
        var result = await _sut.Login(new UserLogin());
        Assert.IsType<NotFoundObjectResult>(result);
    }
}