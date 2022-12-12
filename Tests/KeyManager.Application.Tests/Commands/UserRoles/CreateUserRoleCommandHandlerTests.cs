using System;
using System.Linq.Expressions;
using KeyManager.Application.Commands.UserRoles;
using KeyManager.Domain.Entities;

namespace KeyManager.Application.Tests.Commands.UserRoles;

public class CreateUserRoleCommandHandlerTests
{
    private readonly Mock<IGenericRepository<Role>> _mockRoleRepository;
    private readonly Mock<IGenericRepository<User>> _mockUserRepository;
    private readonly Mock<IGenericRepository<UserRole>> _mockUserRoleRepository;
    private readonly CreateUserRoleCommandHandler _userRoleHandler;

    public CreateUserRoleCommandHandlerTests()
    {
        _mockUserRoleRepository = new Mock<IGenericRepository<UserRole>>();
        _mockUserRepository = new Mock<IGenericRepository<User>>();
        _mockRoleRepository = new Mock<IGenericRepository<Role>>();
        _mockUserRepository.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(new User { Name = "test" });
        _mockRoleRepository.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(new Role { Name = "test" });

        var myProfile = new AutoMapperProfile();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        var mapper = new Mapper(configuration);
        _userRoleHandler = new CreateUserRoleCommandHandler(_mockUserRoleRepository.Object,
            _mockUserRepository.Object, _mockRoleRepository.Object, mapper);
    }

    [Fact]
    public async Task Given_UserRole_When_CreateUserRole_Then_ReturnsCreateUserRoleDto()
    {
        //Arrange
        var newUserRole = new UserRole
        {
            Id = 1,
            RoleId = 1,
            UserId = 1
        };
        _mockUserRoleRepository.Setup(s => s.AddAsync(It.IsAny<UserRole>())).ReturnsAsync(newUserRole);

        //Act
        var result =
            await _userRoleHandler.Handle(
                new CreateUserRoleCommand(newUserRole.UserId, newUserRole.RoleId), default);

        //Assert
        Assert.Equal(result.Id, newUserRole.Id);
        Assert.Equal(result.UserId, newUserRole.UserId);
        Assert.Equal(result.RoleId, newUserRole.RoleId);

        _mockUserRoleRepository.VerifyAll();
    }

    [Fact]
    public async Task Given_UserRole_When_CreateUserRoleWithSameIds_Then_ThrowsExistingRecordException()
    {
        //Arrange
        var mockUserRole = new UserRole
        {
            Id = 1001,
            UserId = 1,
            RoleId = 1
        };
        var mockCreateUserRoleCommand = new CreateUserRoleCommand(mockUserRole.UserId, mockUserRole.RoleId);
        _mockUserRoleRepository.Setup(s => s.GetAsync(It.IsAny<Expression<Func<UserRole, bool>>>()))
            .ReturnsAsync(mockUserRole);

        //Act
        Task Result()
        {
            return _userRoleHandler.Handle(mockCreateUserRoleCommand, default);
        }

        //Assert
        var exception = await Assert.ThrowsAsync<RecordAlreadyExistsException>(Result);
        Assert.Equal("There is a userRole with the same User ID and Role ID: " +
                     $"User ID: '{mockUserRole.UserId}', Role ID: '{mockUserRole.RoleId}'", exception.Message);
        _mockUserRoleRepository.VerifyAll();
    }

    [Fact]
    public async Task Given_UserRole_When_CreateUserRoleWithWrongUserId_Then_ThrowsUserNotFoundException()
    {
        //Arrange
        _mockUserRepository.Setup(s => s.GetByIdAsync(2)).ReturnsAsync((User)null);

        //Act
        Task Result()
        {
            return _userRoleHandler.Handle(new CreateUserRoleCommand(2, 2), default);
        }

        //Assert
        var exception = await Assert.ThrowsAsync<RecordNotFoundException>(Result);
        Assert.Equal("User not found. User ID: '2'", exception.Message);
        _mockUserRoleRepository.VerifyAll();
    }

    [Fact]
    public async Task Given_UserRole_When_CreateUserRoleWithWrongRole_Then_ThrowsRoleNotFoundException()
    {
        //Arrange
        _mockRoleRepository.Setup(s => s.GetByIdAsync(2)).ReturnsAsync((Role)null);

        //Act
        Task Result()
        {
            return _userRoleHandler.Handle(new CreateUserRoleCommand(1, 2), default);
        }

        //Assert
        var exception = await Assert.ThrowsAsync<RecordNotFoundException>(Result);
        Assert.Equal("Role not found. Role ID: '2'", exception.Message);
        _mockUserRoleRepository.VerifyAll();
    }
}