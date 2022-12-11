using System;
using System.Linq.Expressions;
using KeyManager.Application.Commands.Permissions;
using KeyManager.Domain.Entities;

namespace KeyManager.Application.Tests.Commands.Permissions;

public class CreatePermissionCommandHandlerTests
{
    private readonly Mock<IGenericRepository<Door>> _mockDoorRepository;
    private readonly Mock<IGenericRepository<Permission>> _mockPermissionRepository;
    private readonly Mock<IGenericRepository<User>> _mockUserRepository;
    private readonly CreatePermissionCommandHandler _permissionHandler;

    public CreatePermissionCommandHandlerTests()
    {
        _mockPermissionRepository = new Mock<IGenericRepository<Permission>>();
        _mockUserRepository = new Mock<IGenericRepository<User>>();
        _mockDoorRepository = new Mock<IGenericRepository<Door>>();
        _mockUserRepository.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(new User { Name = "test" });
        _mockDoorRepository.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(new Door { Name = "test" });

        var myProfile = new AutoMapperProfile();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        var mapper = new Mapper(configuration);
        _permissionHandler = new CreatePermissionCommandHandler(_mockPermissionRepository.Object,
            _mockUserRepository.Object, _mockDoorRepository.Object, mapper);
    }

    [Fact]
    public async Task Given_Permission_When_CreatePermission_Then_ReturnsCreatePermissionDto()
    {
        //Arrange
        var newPermission = new Permission
        {
            Id = 1,
            DoorId = 1,
            UserId = 1
        };
        _mockPermissionRepository.Setup(s => s.AddAsync(It.IsAny<Permission>())).ReturnsAsync(newPermission);

        //Act
        var result =
            await _permissionHandler.Handle(
                new CreatePermissionCommand(newPermission.UserId, newPermission.DoorId), default);

        //Assert
        Assert.Equal(result.Id, newPermission.Id);
        Assert.Equal(result.UserId, newPermission.UserId);
        Assert.Equal(result.DoorId, newPermission.DoorId);

        _mockPermissionRepository.VerifyAll();
    }

    [Fact]
    public async Task Given_Permission_When_CreatePermissionWithSameIds_Then_ThrowsExistingRecordException()
    {
        //Arrange
        var mockPermission = new Permission
        {
            Id = 1001,
            UserId = 1,
            DoorId = 1
        };
        var mockCreatePermissionCommand = new CreatePermissionCommand(mockPermission.UserId, mockPermission.DoorId);
        _mockPermissionRepository.Setup(s => s.GetAsync(It.IsAny<Expression<Func<Permission, bool>>>()))
            .ReturnsAsync(mockPermission);

        //Act
        Task Result()
        {
            return _permissionHandler.Handle(mockCreatePermissionCommand, default);
        }

        //Assert
        var exception = await Assert.ThrowsAsync<PermissionException>(Result);
        Assert.Equal("There is a permission with the same User ID and Door ID: " +
                     $"User ID: '{mockPermission.UserId}', Door ID: '{mockPermission.DoorId}'", exception.Message);
        _mockPermissionRepository.VerifyAll();
    }

    [Fact]
    public async Task Given_Permission_When_CreatePermissionWithWrongUserId_Then_ThrowsUserNotFoundException()
    {
        //Arrange
        _mockUserRepository.Setup(s => s.GetByIdAsync(2)).ReturnsAsync((User)null);

        //Act
        Task Result()
        {
            return _permissionHandler.Handle(new CreatePermissionCommand(2, 2), default);
        }

        //Assert
        var exception = await Assert.ThrowsAsync<PermissionException>(Result);
        Assert.Equal("User not found. User ID: '2'", exception.Message);
        _mockPermissionRepository.VerifyAll();
    }

    [Fact]
    public async Task Given_Permission_When_CreatePermissionWithWrongDoor_Then_ThrowsDoorNotFoundException()
    {
        //Arrange
        _mockDoorRepository.Setup(s => s.GetByIdAsync(2)).ReturnsAsync((Door)null);

        //Act
        Task Result()
        {
            return _permissionHandler.Handle(new CreatePermissionCommand(1, 2), default);
        }

        //Assert
        var exception = await Assert.ThrowsAsync<PermissionException>(Result);
        Assert.Equal("Door not found. Door ID: '2'", exception.Message);
        _mockPermissionRepository.VerifyAll();
    }
}