using KeyManager.Application.Commands.Users;
using KeyManager.Domain.Entities;

namespace KeyManager.Application.Tests.Commands.Users;

public class CreateUserCommandHandlerTests
{
    private readonly Mock<IGenericRepository<User>> _mockUserRepository;
    private readonly CreateUserCommandHandler _userHandler;

    public CreateUserCommandHandlerTests()
    {
        _mockUserRepository = new Mock<IGenericRepository<User>>();
        var myProfile = new AutoMapperProfile();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        var mapper = new Mapper(configuration);
        _userHandler = new CreateUserCommandHandler(_mockUserRepository.Object, mapper);
    }

    [Fact]
    public async Task Given_User_When_CreateUser_Then_ReturnsCreateUserDto()
    {
        //Arrange
        var newUser = new User
        {
            Id = 1,
            Name = "New Name",
            Surname = "New Surname",
            Username = "New Username",
            Password = "New Password"
        };
        _mockUserRepository.Setup(s => s.AddAsync(It.IsAny<User>())).ReturnsAsync(newUser);

        //Act
        var result =
            await _userHandler.Handle(
                new CreateUserCommand(newUser.Username, newUser.Name, newUser.Surname, newUser.Password), default);

        //Assert
        Assert.Equal(result.Id, newUser.Id);
        Assert.Equal(result.Name, newUser.Name);
        Assert.Equal(result.Surname, newUser.Surname);
        Assert.Equal(result.Username, newUser.Username);

        _mockUserRepository.VerifyAll();
    }

    [Fact]
    public async Task Given_User_When_CreateUserWithSameUsername_Then_ThrowsExistingRecordException()
    {
        //Arrange
        var mockCreateUserCommand = new CreateUserCommand("Test Username", "Test", "TestSurname", "TestPassword");
        var mockUser = new User
        {
            Id = 1001,
            Username = "Test Username",
            Name = "Test1",
            Surname = "TestSurname",
            Password = "TestPassword"
        };
        _mockUserRepository.Setup(s => s.GetAsync(p => p.Username == mockCreateUserCommand.Username))
            .ReturnsAsync(mockUser);

        //Act
        Task Result()
        {
            return _userHandler.Handle(mockCreateUserCommand, default);
        }

        //Assert
        var exception = await Assert.ThrowsAsync<RecordAlreadyExistsException>(Result);
        Assert.Equal("There is a user with the same username: 'Test Username'", exception.Message);
        _mockUserRepository.VerifyAll();
    }
}