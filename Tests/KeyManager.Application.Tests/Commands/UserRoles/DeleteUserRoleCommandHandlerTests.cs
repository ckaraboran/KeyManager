using KeyManager.Application.Commands.UserRoles;
using KeyManager.Domain.Entities;

namespace KeyManager.Application.Tests.Commands.UserRoles;

public class DeleteUserRoleCommandHandlerTests
{
    private readonly Mock<IGenericRepository<UserRole>> _mockUserRoleRepository;
    private readonly DeleteUserRoleCommandHandler _userRoleHandler;

    public DeleteUserRoleCommandHandlerTests()
    {
        _mockUserRoleRepository = new Mock<IGenericRepository<UserRole>>();
        _userRoleHandler = new DeleteUserRoleCommandHandler(_mockUserRoleRepository.Object);
    }

    [Fact]
    public async Task UserRole_DeleteAsync_WithGivenId_ShouldBeVerified()
    {
        //Arrange
        var mockUserRole = new UserRole
        {
            Id = 1,
            UserId = 1,
            RoleId = 1
        };
        _mockUserRoleRepository.Setup(s => s.GetByIdAsync(It.IsAny<long>())).ReturnsAsync(mockUserRole);
        _mockUserRoleRepository.Setup(s => s.DeleteAsync(It.IsAny<UserRole>()));

        //Act
        await _userRoleHandler.Handle(new DeleteUserRoleCommand(1), default);

        //Assert
        _mockUserRoleRepository.VerifyAll();
    }

    [Fact]
    public async Task UserRole_DeleteAsync_WithGivenId_ShouldThrowRecordNotFoundException_IfRecordDoesNotExist()
    {
        //Arrange
        _mockUserRoleRepository.Setup(s => s.GetByIdAsync(It.IsAny<long>())).ReturnsAsync((UserRole)null);

        //Act
        Task Result()
        {
            return _userRoleHandler.Handle(new DeleteUserRoleCommand(5), default);
        }

        //Act-Assert
        var exception = await Assert.ThrowsAsync<RecordNotFoundException>(Result);
        Assert.Equal("UserRole not found while deleting. UserRoleId: '5'", exception.Message);
        _mockUserRoleRepository.VerifyAll();
    }
}