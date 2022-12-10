using KeyManager.Application.Commands.Users;
using KeyManager.Domain.Entities;

namespace KeyManager.Application.Tests.Commands.Users;

public class UpdateUserCommandHandlerTests
{
    private readonly Mock<IGenericRepository<User>> _mockUserRepository;
    private readonly UpdateUserCommandHandler _userHandler;

    public UpdateUserCommandHandlerTests()
    {
        _mockUserRepository = new Mock<IGenericRepository<User>>();
        var myProfile = new AutoMapperProfile();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        var mapper = new Mapper(configuration);
        _userHandler = new UpdateUserCommandHandler(_mockUserRepository.Object, mapper);
    }

    [Fact]
    public async Task Given_User_When_UpdateUser_Then_ReturnsUpdateUserDto()
    {
        //Arrange
        var mockUserUpdateCommand = new UpdateUserCommand(1, 1001, "Test", "TestSurname", 1);

        var updatedUser = new User
        {
            EmployeeId = 1001,
            Id = 1,
            Name = "OldUser",
            Surname = "OldSurname",
            RoleId = 1
        };
        _mockUserRepository.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(updatedUser);
        _mockUserRepository.Setup(s => s.UpdateAsync(It.IsAny<User>())).ReturnsAsync(updatedUser);

        //Act
        var result = await _userHandler.Handle(mockUserUpdateCommand, default);

        //Assert
        Assert.Equal(result.Id, updatedUser.Id);
        Assert.Equal(result.Name, updatedUser.Name);
        _mockUserRepository.VerifyAll();
    }

    [Fact]
    public async Task
        Given_User_When_UpdateUserNotFound_Then_ThrowsUserNotFoundException()
    {
        //Arrange
        var mockUpdateUserCommand = new UpdateUserCommand(1, 1001, "Test", "TestSurname", 1);
        _mockUserRepository.Setup(s => s.GetByIdAsync(mockUpdateUserCommand.Id))
            .ReturnsAsync((User)null);

        //Act
        Task Result()
        {
            return _userHandler.Handle(mockUpdateUserCommand, default);
        }

        //Assert
        var exception = await Assert.ThrowsAsync<UserException>(Result);
        Assert.Equal("User is not found. UserId: '1'", exception.Message);
        _mockUserRepository.VerifyAll();
    }

    [Fact]
    public async Task Given_User_When_UpdateEmployeeIsDifferent_Then_ThrowsEmployeeDifferentException()
    {
        //Arrange
        var mockUserUpdateCommand = new UpdateUserCommand(1, 1001, "Test", "TestSurname", 1);

        var updatedUser = new User
        {
            EmployeeId = 1002,
            Id = 1,
            Name = "OldUser",
            Surname = "OldSurname",
            RoleId = 1
        };
        _mockUserRepository.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(updatedUser);

        //Act
        Task Result()
        {
            return _userHandler.Handle(mockUserUpdateCommand, default);
        }

        //Assert
        var exception = await Assert.ThrowsAsync<UserException>(Result);
        Assert.Equal("Employee ID cannot be different then current one. " +
                     "EmployeeId: '1001'", exception.Message);
        _mockUserRepository.VerifyAll();
    }
}