using KeyManager.Application.Commands.Users;
using KeyManager.Domain.Entities;

namespace KeyManager.Application.Tests.Commands.Users;

public class CreateUserCommandHandlerTests
{
    private readonly Mock<IGenericRepository<Role>> _mockRoleRepository;
    private readonly Mock<IGenericRepository<User>> _mockUserRepository;
    private readonly CreateUserCommandHandler _userHandler;

    public CreateUserCommandHandlerTests()
    {
        _mockUserRepository = new Mock<IGenericRepository<User>>();
        _mockRoleRepository = new Mock<IGenericRepository<Role>>();
        _mockRoleRepository.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(new Role { Name = "test" });
        var myProfile = new AutoMapperProfile();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        var mapper = new Mapper(configuration);
        _userHandler = new CreateUserCommandHandler(_mockUserRepository.Object, _mockRoleRepository.Object, mapper);
    }

    [Fact]
    public async Task User_Create_WithGivenCreateUserCommand_ShouldReturnCreateUserDto()
    {
        //Arrange
        var mockUserDto = new UserDto
        {
            Id = 1,
            Name = "Test"
        };
        var mockUser = new User
        {
            Id = 1,
            Name = "Test"
        };
        _mockUserRepository.Setup(s => s.AddAsync(It.IsAny<User>())).ReturnsAsync(mockUser);

        //Act
        var result = await _userHandler.Handle(new CreateUserCommand(1001, "Test", "TestSurname", 1), default);

        //Assert
        Assert.Equal(result.Id, mockUserDto.Id);
        Assert.Equal(result.Name, mockUserDto.Name);

        _mockUserRepository.VerifyAll();
    }

    [Fact]
    public async Task Given_User_When_CreateUserWithSameEmployeeId_Then_ThrowsExistingRecordException()
    {
        //Arrange
        var mockCreateUserCommand = new CreateUserCommand(1001, "Test", "TestSurname", 1);
        var mockUser = new User
        {
            Id = 1001,
            Name = "Test1",
            Surname = "TestSurname",
            RoleId = 1
        };
        _mockUserRepository.Setup(s => s.GetAsync(p => p.EmployeeId == mockCreateUserCommand.EmployeeId))
            .ReturnsAsync(mockUser);

        //Act
        Task Result()
        {
            return _userHandler.Handle(mockCreateUserCommand, default);
        }

        //Assert
        var exception = await Assert.ThrowsAsync<UserException>(Result);
        Assert.Equal("There is a user with the same employee ID: 'Test'", exception.Message);
        _mockUserRepository.VerifyAll();
    }

    [Fact]
    public async Task Given_User_When_CreateUserWithWrongRoleId_Then_ThrowsRoleNotFoundException()
    {
        //Arrange
        _mockRoleRepository.Setup(s => s.GetByIdAsync(2)).ReturnsAsync((Role)null);

        //Act
        Task Result()
        {
            return _userHandler.Handle(new CreateUserCommand(1001, "Test", "TestSurname", 2), default);
        }

        //Assert
        var exception = await Assert.ThrowsAsync<UserException>(Result);
        Assert.Equal("Role not found. Role ID: '2'", exception.Message);
        _mockUserRepository.VerifyAll();
    }
}