using System;
using System.Linq.Expressions;
using KeyManager.Application.Commands.UserRoles;
using KeyManager.Domain.Entities;

namespace KeyManager.Application.Tests.Commands.UserRoles;

public class UpdateUserRoleCommandHandlerTests
{
    private readonly Mock<IGenericRepository<Role>> _mockRoleRepository;
    private readonly Mock<IGenericRepository<User>> _mockUserRepository;
    private readonly Mock<IGenericRepository<UserRole>> _mockUserRoleRepository;
    private readonly UpdateUserRoleCommandHandler _userRoleHandler;

    public UpdateUserRoleCommandHandlerTests()
    {
        _mockUserRoleRepository = new Mock<IGenericRepository<UserRole>>();
        _mockUserRepository = new Mock<IGenericRepository<User>>();
        _mockRoleRepository = new Mock<IGenericRepository<Role>>();
        _mockUserRepository.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(new User { Name = "test" });
        _mockRoleRepository.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(new Role { Name = "test" });
        var myProfile = new AutoMapperProfile();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        var mapper = new Mapper(configuration);
        _userRoleHandler = new UpdateUserRoleCommandHandler(_mockUserRoleRepository.Object,
            _mockUserRepository.Object, _mockRoleRepository.Object, mapper);
    }

    [Fact]
    public async Task Given_UserRole_When_UpdateUserRole_Then_ReturnsUpdateUserRoleDto()
    {
        //Arrange
        var oldUserRole = new UserRole
        {
            Id = 1,
            RoleId = 1,
            UserId = 1
        };

        var newUserRole = new UserRole
        {
            Id = oldUserRole.Id,
            RoleId = 2,
            UserId = 2
        };
        _mockUserRepository.Setup(s => s.GetByIdAsync(2)).ReturnsAsync(new User { Name = "test" });
        _mockRoleRepository.Setup(s => s.GetByIdAsync(2)).ReturnsAsync(new Role { Name = "test" });
        _mockUserRoleRepository.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(oldUserRole);
        _mockUserRoleRepository.Setup(s => s.UpdateAsync(It.IsAny<UserRole>())).ReturnsAsync(newUserRole);
        var mockUserRoleUpdateCommand =
            new UpdateUserRoleCommand(newUserRole.Id, newUserRole.RoleId, newUserRole.UserId);
        //Act
        var result = await _userRoleHandler.Handle(mockUserRoleUpdateCommand, default);

        //Assert
        Assert.Equal(result.Id, newUserRole.Id);
        Assert.Equal(result.RoleId, newUserRole.RoleId);
        Assert.Equal(result.UserId, newUserRole.UserId);
        _mockUserRoleRepository.VerifyAll();
    }

    [Fact]
    public async Task
        Given_UserRole_When_UpdateUserRoleNotFound_Then_ThrowsUserRoleNotFoundException()
    {
        //Arrange
        var mockUpdateUserRoleCommand = new UpdateUserRoleCommand(2, 1001, 1001);
        _mockUserRoleRepository.Setup(s => s.GetByIdAsync(mockUpdateUserRoleCommand.Id))
            .ReturnsAsync((UserRole)null);

        //Act
        Task Result()
        {
            return _userRoleHandler.Handle(mockUpdateUserRoleCommand, default);
        }

        //Assert
        var exception = await Assert.ThrowsAsync<UserRoleException>(Result);
        Assert.Equal($"UserRole not found. UserRoleId: '{mockUpdateUserRoleCommand.Id}'",
            exception.Message);
        _mockUserRoleRepository.VerifyAll();
    }

    [Fact]
    public async Task Given_UserRole_When_UpdateUserRoleValuesSameAsOtherOne_Then_ThrowsExistingValuesException()
    {
        //Arrange
        var updatedUserRole = new UserRole
        {
            Id = 1,
            RoleId = 1,
            UserId = 1
        };
        var existingOtherUserRole = new UserRole
        {
            Id = 2,
            RoleId = 1,
            UserId = 1
        };
        _mockUserRoleRepository.Setup(s => s
            .GetByIdAsync(1)).ReturnsAsync(updatedUserRole);
        _mockUserRoleRepository.Setup(s => s
            .GetAsync(It.IsAny<Expression<Func<UserRole, bool>>>())).ReturnsAsync(existingOtherUserRole);

        //Act
        Task Result()
        {
            return _userRoleHandler.Handle(
                new UpdateUserRoleCommand(updatedUserRole.Id,
                    updatedUserRole.UserId, updatedUserRole.RoleId), default);
        }

        //Assert
        var exception = await Assert.ThrowsAsync<UserRoleException>(Result);
        Assert.Equal("There is a userRole with the same User ID and Role ID: " +
                     $"User ID: '{updatedUserRole.UserId}', Role ID: '{updatedUserRole.RoleId}'",
            exception.Message);
        _mockUserRoleRepository.VerifyAll();
    }

    [Fact]
    public async Task Given_UserRole_When_UpdateUserRoleWithWrongUserId_Then_ThrowsUserNotFoundException()
    {
        //Arrange
        var existingUserRole = new UserRole
        {
            Id = 1,
            RoleId = 1,
            UserId = 1
        };
        _mockUserRoleRepository.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(existingUserRole);
        _mockUserRepository.Setup(s => s.GetByIdAsync(2)).ReturnsAsync((User)null);

        //Act
        Task Result()
        {
            return _userRoleHandler.Handle(new UpdateUserRoleCommand(1, 2, 1), default);
        }

        //Assert
        var exception = await Assert.ThrowsAsync<UserRoleException>(Result);
        Assert.Equal("User not found. User ID: '2'", exception.Message);
        _mockUserRoleRepository.VerifyAll();
    }

    [Fact]
    public async Task Given_UserRole_When_UpdateUserRoleWithWrongRole_Then_ThrowsRoleNotFoundException()
    {
        //Arrange
        var existingUserRole = new UserRole
        {
            Id = 1,
            RoleId = 1,
            UserId = 1
        };
        _mockUserRoleRepository.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(existingUserRole);
        _mockRoleRepository.Setup(s => s.GetByIdAsync(2)).ReturnsAsync((Role)null);

        //Act
        Task Result()
        {
            return _userRoleHandler.Handle(new UpdateUserRoleCommand(1, 1, 2), default);
        }

        //Assert
        var exception = await Assert.ThrowsAsync<UserRoleException>(Result);
        Assert.Equal("Role not found. Role ID: '2'", exception.Message);
        _mockUserRoleRepository.VerifyAll();
    }
}