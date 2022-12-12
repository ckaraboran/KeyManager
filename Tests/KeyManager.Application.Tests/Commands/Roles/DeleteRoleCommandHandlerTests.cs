using KeyManager.Application.Commands.Roles;
using KeyManager.Domain.Entities;

namespace KeyManager.Application.Tests.Commands.Roles;

public class DeleteRoleCommandHandlerTests
{
    private readonly Mock<IGenericRepository<Role>> _mockRoleRepository;
    private readonly DeleteRoleCommandHandler _roleHandler;

    public DeleteRoleCommandHandlerTests()
    {
        _mockRoleRepository = new Mock<IGenericRepository<Role>>();
        _roleHandler = new DeleteRoleCommandHandler(_mockRoleRepository.Object);
    }

    [Fact]
    public async Task Role_DeleteAsync_WithGivenId_ShouldBeVerified()
    {
        //Arrange
        var mockRole = new Role
        {
            Id = 1,
            Name = "Test"
        };
        _mockRoleRepository.Setup(s => s.GetByIdAsync(It.IsAny<long>())).ReturnsAsync(mockRole);
        _mockRoleRepository.Setup(s => s.DeleteAsync(It.IsAny<Role>()));

        //Act
        await _roleHandler.Handle(new DeleteRoleCommand(1), default);

        //Assert
        _mockRoleRepository.VerifyAll();
    }

    [Fact]
    public async Task Role_DeleteAsync_WithGivenId_ShouldThrowRecordNotFoundException_IfRecordDoesNotExist()
    {
        //Arrange
        _mockRoleRepository.Setup(s => s.GetByIdAsync(It.IsAny<long>())).ReturnsAsync((Role)null);

        //Act
        Task Result()
        {
            return _roleHandler.Handle(new DeleteRoleCommand(5), default);
        }

        //Act-Assert
        var exception = await Assert.ThrowsAsync<RecordNotFoundException>(Result);
        Assert.Equal("Role not found while deleting. RoleId: '5'", exception.Message);
        _mockRoleRepository.VerifyAll();
    }
}