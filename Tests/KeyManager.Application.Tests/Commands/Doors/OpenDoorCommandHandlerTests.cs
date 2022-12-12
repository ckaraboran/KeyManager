using System;
using System.Linq.Expressions;
using KeyManager.Application.Commands.Doors;
using KeyManager.Domain.Entities;

namespace KeyManager.Application.Tests.Commands.Doors;

public class OpenDoorCommandHandlerTests
{
    private readonly Mock<IGenericRepository<Door>> _mockDoorRepository;
    private readonly Mock<IGenericRepository<Incident>> _mockIncidentRepository;
    private readonly Mock<IGenericRepository<Permission>> _mockPermissionRepository;
    private readonly Mock<IGenericRepository<User>> _mockUserRepository;
    private readonly OpenDoorCommandHandler _openDoorHandler;

    public OpenDoorCommandHandlerTests()
    {
        _mockIncidentRepository = new Mock<IGenericRepository<Incident>>();
        _mockPermissionRepository = new Mock<IGenericRepository<Permission>>();
        _mockUserRepository = new Mock<IGenericRepository<User>>();
        _mockDoorRepository = new Mock<IGenericRepository<Door>>();
        _mockUserRepository.Setup(s => s.GetAsync(It.IsAny<Expression<Func<User, bool>>>()))
            .ReturnsAsync(new User { Name = "test" });
        _mockDoorRepository.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(new Door { Name = "test" });
        _openDoorHandler = new OpenDoorCommandHandler(_mockDoorRepository.Object,
            _mockUserRepository.Object,
            _mockPermissionRepository.Object, _mockIncidentRepository.Object);
    }

    [Fact]
    public async Task Given_OpenDoorCommand_When_OpenDoor_Then_ReturnsTrue()
    {
        //Arrange
        var newPermission = new OpenDoorCommand("Test", 1);
        _mockPermissionRepository.Setup(s =>
                s.GetAsync(It.IsAny<Expression<Func<Permission, bool>>>()))
            .ReturnsAsync(new Permission { DoorId = 1, UserId = 1 });
        _mockIncidentRepository.Setup(s => s.AddAsync(It.IsAny<Incident>()));
        //Act
        await _openDoorHandler.Handle(newPermission, default);

        //Assert
        _mockPermissionRepository.VerifyAll();
        _mockIncidentRepository.VerifyAll();
    }

    [Fact]
    public async Task Given_OpenDoorCommand_When_OpenDoorDoorNotFound_Then_ReturnsDoorNotFoundException()
    {
        //Arrange
        var newPermission = new OpenDoorCommand("Test", 2);
        _mockDoorRepository.Setup(s => s.GetByIdAsync(2)).ReturnsAsync((Door)null);

        //Act
        Task Result()
        {
            return _openDoorHandler.Handle(newPermission, default);
        }

        //Assert
        var exception = await Assert.ThrowsAsync<DoorException>(Result);
        Assert.Equal($"Door not found. Door ID: '{newPermission.DoorId}'", exception.Message);
    }

    [Fact]
    public async Task Given_OpenDoorCommand_When_OpenDoorUserNotFound_Then_ReturnsUserNotFoundException()
    {
        //Arrange
        var newPermission = new OpenDoorCommand("Test", 1);
        _mockUserRepository.Setup(s =>
            s.GetAsync(It.IsAny<Expression<Func<User, bool>>>())).ReturnsAsync((User)null);

        //Act
        Task Result()
        {
            return _openDoorHandler.Handle(newPermission, default);
        }

        //Assert
        var exception = await Assert.ThrowsAsync<DoorException>(Result);
        Assert.Equal($"User not found. Username: '{newPermission.Username}'", exception.Message);
    }

    [Fact]
    public async Task Given_OpenDoorCommand_When_NotPermitted_Then_ThrowsNotPermittedException()
    {
        //Arrange
        var newPermission = new OpenDoorCommand("Test", 1);
        _mockPermissionRepository.Setup(s =>
                s.GetAsync(It.IsAny<Expression<Func<Permission, bool>>>()))
            .ReturnsAsync((Permission)null);

        //Act
        var result = await _openDoorHandler.Handle(newPermission, default);

        //Assert
        Assert.False(result);
        _mockPermissionRepository.VerifyAll();
    }
}