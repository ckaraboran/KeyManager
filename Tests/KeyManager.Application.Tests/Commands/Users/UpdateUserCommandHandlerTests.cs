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
        var mockUserUpdateCommand = new UpdateUserCommand(1, "Test", "TestSurname");

        var newUser = new User
        {
            Username = "Test Username",
            Id = 1,
            Name = "Old User",
            Surname = "Old Surname"
        };

        var oldUser = new User
        {
            Username = "Test Username",
            Id = 1,
            Name = "Old User",
            Surname = "Old Surname"
        };
        _mockUserRepository.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(oldUser);
        _mockUserRepository.Setup(s => s.UpdateAsync(It.IsAny<User>())).ReturnsAsync(newUser);

        //Act
        var result = await _userHandler.Handle(mockUserUpdateCommand, default);

        //Assert
        Assert.Equal(result.Id, newUser.Id);
        Assert.Equal(result.Name, newUser.Name);
        Assert.Equal(result.Surname, newUser.Surname);
        Assert.Equal(result.Username, newUser.Username);
        _mockUserRepository.VerifyAll();
    }

    [Fact]
    public async Task
        Given_User_When_UpdateUserNotFound_Then_ThrowsUserNotFoundException()
    {
        //Arrange
        var mockUpdateUserCommand = new UpdateUserCommand(1, "Test", "TestSurname");
        _mockUserRepository.Setup(s => s.GetByIdAsync(mockUpdateUserCommand.Id))
            .ReturnsAsync((User)null);

        //Act
        Task Result()
        {
            return _userHandler.Handle(mockUpdateUserCommand, default);
        }

        //Assert
        var exception = await Assert.ThrowsAsync<RecordNotFoundException>(Result);
        Assert.Equal("User not found. UserId: '1'", exception.Message);
        _mockUserRepository.VerifyAll();
    }
}