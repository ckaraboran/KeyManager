using System;
using System.Linq.Expressions;
using KeyManager.Application.Commands.Roles;
using KeyManager.Domain.Entities;
using KeyManager.Domain.Enums;

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
    public async Task Given_RolePut_When_WithGivenUpdateRoleRequest_Then_ReturnCreateRoleDto()
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
    public async Task Given_RolePut_When_AnotherRoleWithSameNameExists_Then_RecordAlreadyExistsException()
    {
        //Arrange
        var sameName = "New Role";
        var newRole = new Role
        {
            Id = 1,
            Name = sameName
        };
        var newAnotherRole = new Role
        {
            Id = 2,
            Name = sameName
        };
        var mockRoleUpdateCommand = new UpdateRoleCommand(newRole.Id, newRole.Name);

        var oldRole = new Role
        {
            Id = 1,
            Name = "Old Role"
        };

        _mockRoleRepository.Setup(s => s.GetByIdAsync(newRole.Id)).ReturnsAsync(oldRole);
        _mockRoleRepository.Setup(s => s.GetAsync(It.IsAny<Expression<Func<Role, bool>>>()))
            .ReturnsAsync(newAnotherRole);
        _mockRoleRepository.Setup(s => s.UpdateAsync(It.IsAny<Role>())).ReturnsAsync(newRole);

        Task Result()
        {
            return _roleHandler.Handle(mockRoleUpdateCommand, default);
        }

        //Assert
        var exception = await Assert.ThrowsAsync<RecordAlreadyExistsException>(Result);
        Assert.Equal($"There is a role with the same name: '{newAnotherRole.Name}'", exception.Message);
        _mockRoleRepository.Verify(s => s.UpdateAsync(It.IsAny<Role>()), Times.Never);
    }

    [Fact]
    public async Task Given_RolePut_When_RoleIsPredefined_Then_RecordACannotBeChangedException()
    {
        //Arrange
        var sameName = "New Role";
        var newRole = new Role
        {
            Id = 1,
            Name = sameName
        };
        var newAnotherRole = new Role
        {
            Id = 2,
            Name = sameName
        };
        var mockRoleUpdateCommand = new UpdateRoleCommand(newRole.Id, newRole.Name);

        var oldRole = new Role
        {
            Id = 1,
            Name = KnownRoles.OfficeManager.ToString()
        };

        _mockRoleRepository.Setup(s => s.GetByIdAsync(newRole.Id)).ReturnsAsync(oldRole);
        _mockRoleRepository.Setup(s => s.GetAsync(It.IsAny<Expression<Func<Role, bool>>>()))
            .ReturnsAsync(newAnotherRole);
        _mockRoleRepository.Setup(s => s.UpdateAsync(It.IsAny<Role>())).ReturnsAsync(newRole);

        Task Result()
        {
            return _roleHandler.Handle(mockRoleUpdateCommand, default);
        }

        //Assert
        var exception = await Assert.ThrowsAsync<RecordCannotBeChangedException>(Result);
        Assert.Equal($"Predefined role cannot be changed. Role name: '{oldRole.Name}'", exception.Message);
        _mockRoleRepository.Verify(s => s.UpdateAsync(It.IsAny<Role>()), Times.Never);
    }

    [Fact]
    public async Task Given_RolePut_When_RecordDoesNotExist_Then_ThrowRecordNotFoundException()
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