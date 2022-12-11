using System;
using System.Linq.Expressions;
using KeyManager.Application.Commands.UserRoles;
using KeyManager.Domain.Entities;

namespace KeyManager.Application.Tests.Commands.UserRoles;

public class CheckUserForRoleCommandHandlerTests
{
    private readonly Mock<IGenericRepository<Role>> _mockRoleRepository;
    private readonly Mock<IGenericRepository<UserRole>> _mockUserRoleRepository;
    private readonly Mock<IGenericRepository<User>> _mockUserRepository;
    private readonly CheckUserForRoleCommandHandler _openRoleHandler;

    public CheckUserForRoleCommandHandlerTests()
    {
        _mockUserRoleRepository = new Mock<IGenericRepository<UserRole>>();
        _mockUserRepository = new Mock<IGenericRepository<User>>();
        _mockRoleRepository = new Mock<IGenericRepository<Role>>();
        _mockUserRepository.Setup(s => s.GetAsync(It.IsAny<Expression<Func<User,bool>>>())).ReturnsAsync(new User { Name = "test" });
        _mockRoleRepository.Setup(s => s.GetAsync(It.IsAny<Expression<Func<Role,bool>>>())).ReturnsAsync(new Role { Name = "test" });
        _openRoleHandler = new CheckUserForRoleCommandHandler(_mockUserRoleRepository.Object, 
            _mockUserRepository.Object,
            _mockRoleRepository.Object);
    }

    [Fact]
    public async Task Given_CheckUserForRoleCommand_When_OpenRole_Then_ReturnsTrue()
    {
        //Arrange
        var newUserRole = new CheckUserForRoleCommand("test", "test");
        _mockUserRoleRepository.Setup(s =>
                s.GetAsync(It.IsAny<Expression<Func<UserRole, bool>>>()))
            .ReturnsAsync(new UserRole { RoleId = 1, UserId = 1 });
        //Act
        await _openRoleHandler.Handle(newUserRole, default);
        
        //Assert
        _mockUserRoleRepository.VerifyAll();
        _mockUserRepository.VerifyAll();
    }

    [Fact]
    public async Task Given_CheckUserForRoleCommand_When_RoleNotFound_Then_ReturnsRoleNotFoundException()
    {
        //Arrange
        var newUserRole = new CheckUserForRoleCommand("test", "test");
        _mockRoleRepository.Setup(s => 
            s.GetAsync(It.IsAny<Expression<Func<Role,bool>>>())).ReturnsAsync((Role)null);

        //Act
        Task Result()
        {
            return _openRoleHandler.Handle(newUserRole, default);
        }

        //Assert
        var exception = await Assert.ThrowsAsync<RoleException>(Result);
        Assert.Equal($"Role not found. Role name: '{newUserRole.RoleName}'", exception.Message);
    }

    [Fact]
    public async Task Given_OpenRoleCommand_When_UserNotFound_Then_ReturnsUserNotFoundException()
    {
        //Arrange
        var newUserRole = new CheckUserForRoleCommand("test", "test");
        _mockUserRepository.Setup(s => 
            s.GetAsync(It.IsAny<Expression<Func<User,bool>>>())).ReturnsAsync((User)null);

        //Act
        Task Result()
        {
            return _openRoleHandler.Handle(newUserRole, default);
        }

        //Assert
        var exception = await Assert.ThrowsAsync<RoleException>(Result);
        Assert.Equal($"User not found. Username: '{newUserRole.RoleName}'", exception.Message);
    }

    [Fact]
    public async Task Given_OpenRoleCommand_When_NotPermitted_Then_ThrowsNotPermittedException()
    {
        //Arrange
        var newUserRole = new CheckUserForRoleCommand("test", "test");
        _mockUserRoleRepository.Setup(s =>
                s.GetAsync(It.IsAny<Expression<Func<UserRole, bool>>>()))
            .ReturnsAsync((UserRole)null);

        //Act
        Task Result()
        {
            return _openRoleHandler.Handle(newUserRole, default);
        }

        //Assert
        var exception = await Assert.ThrowsAsync<RoleException>(Result);
        Assert.Equal("User has no permission to use the role. " +
                     $"Username: '{newUserRole.UserName}', Role name: '{newUserRole.RoleName}'", exception.Message);
    }
}