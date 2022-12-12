using KeyManager.Application.Commands.Roles;
using KeyManager.Domain.Entities;

namespace KeyManager.Application.Tests.Commands.Roles;

public class UpdateRoleCommandHandlerTests
{
    private readonly Mock<IGenericRepository<Role>> _mockRoleRepository;
    private readonly UpdateRoleCommandHandler _roleHandler;

    public UpdateRoleCommandHandlerTests()
    {
        _mockRoleRepository = new Mock<IGenericRepository<Role>>();
        var myProfile = new AutoMapperProfile();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        var mapper = new Mapper(configuration);
        _roleHandler = new UpdateRoleCommandHandler(_mockRoleRepository.Object, mapper);
    }

    [Fact]
    public async Task Role_PutAsync_WithGivenUpdateRoleRequest_ShouldReturnCreateRoleDto()
    {
        //Arrange
        var newRole = new Role
        {
            Id = 1,
            Name = "New Role"
        };
        var mockRoleUpdateCommand = new UpdateRoleCommand(1, newRole.Name);

        var oldRole = new Role
        {
            Id = 1,
            Name = "Old role"
        };

        _mockRoleRepository.Setup(s => s.GetByIdAsync(It.IsAny<long>())).ReturnsAsync(oldRole);
        _mockRoleRepository.Setup(s => s.UpdateAsync(It.IsAny<Role>())).ReturnsAsync(newRole);

        //Act
        var result = await _roleHandler.Handle(mockRoleUpdateCommand, default);

        //Assert
        Assert.Equal(result.Id, newRole.Id);
        Assert.Equal(result.Name, newRole.Name);
        _mockRoleRepository.VerifyAll();
    }

    [Fact]
    public async Task
        Role_PutAsync_WithGivenUpdateRoleRequest_ShouldThrowRecordNotFoundException_IfRecordDoesNotExist()
    {
        //Arrange
        var mockUpdateRoleCommand = new UpdateRoleCommand(1, "Test");
        _mockRoleRepository.Setup(s => s.GetByIdAsync(mockUpdateRoleCommand.Id))
            .ReturnsAsync((Role)null);

        //Act
        Task Result()
        {
            return _roleHandler.Handle(mockUpdateRoleCommand, default);
        }

        //Assert
        var exception = await Assert.ThrowsAsync<RecordNotFoundException>(Result);
        Assert.Equal("Role not found. RoleId: '1'", exception.Message);
        _mockRoleRepository.VerifyAll();
    }
}