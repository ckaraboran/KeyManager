using KeyManager.Application.Commands.Permissions;
using KeyManager.Domain.Entities;

namespace KeyManager.Application.Tests.Commands.Permissions;

public class DeletePermissionCommandHandlerTests
{
    private readonly Mock<IGenericRepository<Permission>> _mockPermissionRepository;
    private readonly DeletePermissionCommandHandler _permissionHandler;

    public DeletePermissionCommandHandlerTests()
    {
        _mockPermissionRepository = new Mock<IGenericRepository<Permission>>();
        _permissionHandler = new DeletePermissionCommandHandler(_mockPermissionRepository.Object);
    }

    [Fact]
    public async Task Permission_DeleteAsync_WithGivenId_ShouldBeVerified()
    {
        //Arrange
        var mockPermission = new Permission
        {
            Id = 1,
            UserId = 1,
            DoorId = 1
        };
        _mockPermissionRepository.Setup(s => s.GetByIdAsync(It.IsAny<long>())).ReturnsAsync(mockPermission);
        _mockPermissionRepository.Setup(s => s.DeleteAsync(It.IsAny<Permission>()));

        //Act
        await _permissionHandler.Handle(new DeletePermissionCommand(1), default);

        //Assert
        _mockPermissionRepository.VerifyAll();
    }

    [Fact]
    public async Task Permission_DeleteAsync_WithGivenId_ShouldThrowRecordNotFoundException_IfRecordDoesNotExist()
    {
        //Arrange
        _mockPermissionRepository.Setup(s => s.GetByIdAsync(It.IsAny<long>())).ReturnsAsync((Permission)null);

        //Act
        Task Result()
        {
            return _permissionHandler.Handle(new DeletePermissionCommand(5), default);
        }

        //Act-Assert
        var exception = await Assert.ThrowsAsync<PermissionException>(Result);
        Assert.Equal("Permission not found while deleting. PermissionId: '5'", exception.Message);
        _mockPermissionRepository.VerifyAll();
    }
}