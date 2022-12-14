using KeyManager.Application.Commands.Users;
using KeyManager.Domain.Entities;

namespace KeyManager.Application.Tests.Commands.Users;

public class DeleteUserCommandHandlerTests
{
    private readonly Mock<IGenericRepository<User>> _mockUserRepository;
    private readonly DeleteUserCommandHandler _userHandler;

    public DeleteUserCommandHandlerTests()
    {
        _mockUserRepository = new Mock<IGenericRepository<User>>();
        _userHandler = new DeleteUserCommandHandler(_mockUserRepository.Object);
    }

    [Fact]
    public async Task Given_UserDelete_When_WithGivenId_Then_BeVerified()
    {
        //Arrange
        var mockUser = new User
        {
            Id = 1,
            Name = "Test"
        };
        _mockUserRepository.Setup(s => s.GetByIdAsync(It.IsAny<long>())).ReturnsAsync(mockUser);
        _mockUserRepository.Setup(s => s.DeleteAsync(It.IsAny<User>()));

        //Act
        await _userHandler.Handle(new DeleteUserCommand(1), default);

        //Assert
        _mockUserRepository.VerifyAll();
    }

    [Fact]
    public async Task Given_UserDelete_When_RecordDoesNotExist_Then_ThrowRecordNotFoundException()
    {
        //Arrange
        _mockUserRepository.Setup(s => s.GetByIdAsync(It.IsAny<long>())).ReturnsAsync((User)null);

        //Act
        Task Result()
        {
            return _userHandler.Handle(new DeleteUserCommand(5), default);
        }

        //Act-Assert
        var exception = await Assert.ThrowsAsync<RecordNotFoundException>(Result);
        Assert.Equal("User not found while deleting. UserId: '5'", exception.Message);
        _mockUserRepository.VerifyAll();
    }
}