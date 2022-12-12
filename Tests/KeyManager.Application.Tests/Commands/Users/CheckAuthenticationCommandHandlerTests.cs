using System;
using System.Linq.Expressions;
using KeyManager.Application.Commands.Users;
using KeyManager.Domain.Entities;
using KeyManager.Infrastructure.Security;

namespace KeyManager.Application.Tests.Commands.Users;

public class CheckAuthenticationCommandHandlerTests
{
    private readonly Mock<IGenericRepository<User>> _mockUserRepository;
    private readonly CheckAuthenticationCommandHandler _userHandler;

    public CheckAuthenticationCommandHandlerTests()
    {
        _mockUserRepository = new Mock<IGenericRepository<User>>();
        _userHandler = new CheckAuthenticationCommandHandler(_mockUserRepository.Object);
    }

    [Fact]
    public async Task Given_ExistingUser_When_CheckAuthentication_Then_ReturnsTrue()
    {
        //Arrange
        var newPassword = "New Password";
        var newUser = new User
        {
            Id = 1,
            Name = "New Name",
            Surname = "New Surname",
            Username = "New Username",
            Password = "New Password"
        };

        newUser.Password = ClayPasswordHasher.HashPassword(newUser, newPassword);
        _mockUserRepository.Setup(s => s.GetAsync(It.IsAny<Expression<Func<User, bool>>>()))
            .ReturnsAsync(newUser);
        //Act
        var result =
            await _userHandler.Handle(
                new CheckAuthenticationCommand(newUser.Username, newPassword), default);

        //Assert
        Assert.True(result);
        _mockUserRepository.VerifyAll();
    }
}