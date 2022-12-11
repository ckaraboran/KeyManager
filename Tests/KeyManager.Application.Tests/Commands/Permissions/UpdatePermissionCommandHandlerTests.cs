using System;
using System.Linq.Expressions;
using KeyManager.Application.Commands.Permissions;
using KeyManager.Domain.Entities;

namespace KeyManager.Application.Tests.Commands.Permissions;

public class UpdatePermissionCommandHandlerTests
{
    private readonly Mock<IGenericRepository<Door>> _mockDoorRepository;
    private readonly Mock<IGenericRepository<Permission>> _mockPermissionRepository;
    private readonly Mock<IGenericRepository<User>> _mockUserRepository;
    private readonly UpdatePermissionCommandHandler _permissionHandler;

    public UpdatePermissionCommandHandlerTests()
    {
        _mockPermissionRepository = new Mock<IGenericRepository<Permission>>();
        _mockUserRepository = new Mock<IGenericRepository<User>>();
        _mockDoorRepository = new Mock<IGenericRepository<Door>>();
        _mockUserRepository.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(new User { Name = "test" });
        _mockDoorRepository.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(new Door { Name = "test" });
        var myProfile = new AutoMapperProfile();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        var mapper = new Mapper(configuration);
        _permissionHandler = new UpdatePermissionCommandHandler(_mockPermissionRepository.Object,
            _mockUserRepository.Object, _mockDoorRepository.Object, mapper);
    }

    [Fact]
    public async Task Given_Permission_When_UpdatePermission_Then_ReturnsUpdatePermissionDto()
    {
        //Arrange
        var oldPermission = new Permission
        {
            Id = 1,
            DoorId = 1,
            UserId = 1
        };

        var newPermission = new Permission
        {
            Id = oldPermission.Id,
            DoorId = 2,
            UserId = 2
        };
        _mockUserRepository.Setup(s => s.GetByIdAsync(2)).ReturnsAsync(new User { Name = "test" });
        _mockDoorRepository.Setup(s => s.GetByIdAsync(2)).ReturnsAsync(new Door { Name = "test" });
        _mockPermissionRepository.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(oldPermission);
        _mockPermissionRepository.Setup(s => s.UpdateAsync(It.IsAny<Permission>())).ReturnsAsync(newPermission);
        var mockPermissionUpdateCommand =
            new UpdatePermissionCommand(newPermission.Id, newPermission.DoorId, newPermission.UserId);
        //Act
        var result = await _permissionHandler.Handle(mockPermissionUpdateCommand, default);

        //Assert
        Assert.Equal(result.Id, newPermission.Id);
        Assert.Equal(result.DoorId, newPermission.DoorId);
        Assert.Equal(result.UserId, newPermission.UserId);
        _mockPermissionRepository.VerifyAll();
    }

    [Fact]
    public async Task
        Given_Permission_When_UpdatePermissionNotFound_Then_ThrowsPermissionNotFoundException()
    {
        //Arrange
        var mockUpdatePermissionCommand = new UpdatePermissionCommand(2, 1001, 1001);
        _mockPermissionRepository.Setup(s => s.GetByIdAsync(mockUpdatePermissionCommand.Id))
            .ReturnsAsync((Permission)null);

        //Act
        Task Result()
        {
            return _permissionHandler.Handle(mockUpdatePermissionCommand, default);
        }

        //Assert
        var exception = await Assert.ThrowsAsync<PermissionException>(Result);
        Assert.Equal($"Permission not found. PermissionId: '{mockUpdatePermissionCommand.Id}'",
            exception.Message);
        _mockPermissionRepository.VerifyAll();
    }

    [Fact]
    public async Task Given_Permission_When_UpdatePermissionValuesSameAsOtherOne_Then_ThrowsExistingValuesException()
    {
        //Arrange
        var updatedPermission = new Permission
        {
            Id = 1,
            DoorId = 1,
            UserId = 1
        };
        var existingOtherPermission = new Permission
        {
            Id = 2,
            DoorId = 1,
            UserId = 1
        };
        _mockPermissionRepository.Setup(s => s
            .GetByIdAsync(1)).ReturnsAsync(updatedPermission);
        _mockPermissionRepository.Setup(s => s
            .GetAsync(It.IsAny<Expression<Func<Permission, bool>>>())).ReturnsAsync(existingOtherPermission);

        //Act
        Task Result()
        {
            return _permissionHandler.Handle(
                new UpdatePermissionCommand(updatedPermission.Id,
                    updatedPermission.UserId, updatedPermission.DoorId), default);
        }

        //Assert
        var exception = await Assert.ThrowsAsync<PermissionException>(Result);
        Assert.Equal("There is a permission with the same User ID and Door ID: " +
                     $"User ID: '{updatedPermission.UserId}', Door ID: '{updatedPermission.DoorId}'",
            exception.Message);
        _mockPermissionRepository.VerifyAll();
    }

    [Fact]
    public async Task Given_Permission_When_UpdatePermissionWithWrongUserId_Then_ThrowsUserNotFoundException()
    {
        //Arrange
        var existingPermission = new Permission
        {
            Id = 1,
            DoorId = 1,
            UserId = 1
        };
        _mockPermissionRepository.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(existingPermission);
        _mockUserRepository.Setup(s => s.GetByIdAsync(2)).ReturnsAsync((User)null);

        //Act
        Task Result()
        {
            return _permissionHandler.Handle(new UpdatePermissionCommand(1, 2, 1), default);
        }

        //Assert
        var exception = await Assert.ThrowsAsync<PermissionException>(Result);
        Assert.Equal("User not found. User ID: '2'", exception.Message);
        _mockPermissionRepository.VerifyAll();
    }

    [Fact]
    public async Task Given_Permission_When_UpdatePermissionWithWrongDoor_Then_ThrowsDoorNotFoundException()
    {
        //Arrange
        var existingPermission = new Permission
        {
            Id = 1,
            DoorId = 1,
            UserId = 1
        };
        _mockPermissionRepository.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(existingPermission);
        _mockDoorRepository.Setup(s => s.GetByIdAsync(2)).ReturnsAsync((Door)null);

        //Act
        Task Result()
        {
            return _permissionHandler.Handle(new UpdatePermissionCommand(1, 1, 2), default);
        }

        //Assert
        var exception = await Assert.ThrowsAsync<PermissionException>(Result);
        Assert.Equal("Door not found. Door ID: '2'", exception.Message);
        _mockPermissionRepository.VerifyAll();
    }
}