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
        var roleName = "Test";
        //Arrange
        var mockRoleUpdateCommand = new UpdateRoleCommand(1, roleName);

        var mockRoleDto = new RoleDto
        {
            Id = 1,
            Name = roleName
        };
        var mockRole = new Role
        {
            Id = 1,
            Name = roleName
        };
        _mockRoleRepository.Setup(s => s.GetByIdAsync(It.IsAny<long>())).ReturnsAsync(mockRole);
        _mockRoleRepository.Setup(s => s.UpdateAsync(It.IsAny<Role>())).ReturnsAsync(mockRole);

        //Act
        var result = await _roleHandler.Handle(mockRoleUpdateCommand, default);

        //Assert
        Assert.Equal(result.Id, mockRoleDto.Id);
        Assert.Equal(result.Name, mockRoleDto.Name);
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
        var exception = await Assert.ThrowsAsync<RoleException>(Result);
        Assert.Equal("Role already exists. RoleId: '1'", exception.Message);
        _mockRoleRepository.VerifyAll();
    }
}