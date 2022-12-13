using System.Threading;
using KeyManager.Api.DTOs.Responses.Users;
using KeyManager.Application.Commands.Users;
using KeyManager.Application.Queries.Users;
using MediatR;

namespace KeyManager.Api.Tests.Controllers;

public class UserControllerTests
{
    private readonly Mock<ISender> _mockMediator;
    private readonly UserController _sut;

    public UserControllerTests()
    {
        _mockMediator = new Mock<ISender>();
        var myProfile = new AutoMapperProfile();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        var mapper = new Mapper(configuration);
        _sut = new UserController(_mockMediator.Object, mapper);
    }

    [Fact]
    public async Task Given_UserGet_When_Get_Then_ReturnAllUsers()
    {
        //Arrange
        var mockUserDto = new List<UserDto>
        {
            new() { Id = 1, Name = "Test" },
            new() { Id = 2, Name = "Test2" }
        };
        var mockGetUsersResponse = new List<GetUserResponse>
        {
            new() { Id = 1, Name = "Test" },
            new() { Id = 2, Name = "Test2" }
        };
        _mockMediator.Setup(s => s.Send(It.IsAny<GetUsersQuery>(), It.Is<CancellationToken>(x => x == default)))
            .ReturnsAsync(mockUserDto);

        //Act
        var result = await _sut.GetAsync();

        //Assert
        var resultObject = (List<GetUserResponse>)Assert.IsType<OkObjectResult>(result.Result).Value;
        Assert.NotNull(resultObject);
        Assert.Equal(mockGetUsersResponse[0].Id, resultObject[0].Id);
        Assert.Equal(mockGetUsersResponse[0].Name, resultObject[0].Name);
        Assert.Equal(mockGetUsersResponse[1].Id, resultObject[1].Id);
        Assert.Equal(mockGetUsersResponse[1].Name, resultObject[1].Name);

        _mockMediator.VerifyAll();
    }

    [Fact]
    public async Task Given_UserGet_When_WithGivenId_Then_ReturnAllUser()
    {
        //Arrange
        var mockUserDto = new UserDto
        {
            Id = 1,
            Name = "Test"
        };
        _mockMediator.Setup(s => s.Send(It.IsAny<GetUserByIdQuery>()
            , It.Is<CancellationToken>(x => x == default))).ReturnsAsync(mockUserDto);

        //Act
        var result = await _sut.GetAsync(1);

        //Assert
        var resultObject = (GetUserResponse)Assert.IsType<OkObjectResult>(result.Result).Value;
        Assert.NotNull(resultObject);
        Assert.Equal(mockUserDto.Id, resultObject.Id);
        Assert.Equal(mockUserDto.Name, resultObject.Name);
        _mockMediator.VerifyAll();
    }

    [Fact]
    public async Task Given_UserPost_When_WithGivenUser_Then_AddUser()
    {
        //Arrange
        var mockCreateUserCommand =
            new CreateUserCommand("Test Username", "Test name", "Test surname", "Test password");
        var mockUserDto = new UserDto
        {
            Id = 1,
            Name = "Test"
        };
        _mockMediator.Setup(s => s.Send(It.IsAny<CreateUserCommand>()
            , It.Is<CancellationToken>(x => x == default))).ReturnsAsync(mockUserDto);

        //Act
        var result = await _sut.PostAsync(mockCreateUserCommand);

        //Assert
        var resultObject = (CreateUserResponse)Assert.IsType<CreatedResult>(result.Result).Value;
        Assert.NotNull(resultObject);
        Assert.Equal(mockUserDto.Id, resultObject.Id);
        Assert.Equal(mockUserDto.Name, resultObject.Name);
        _mockMediator.VerifyAll();
    }

    [Fact]
    public async Task Given_UserPut_When_WithGivenUser_Then_UpdateUser()
    {
        //Arrange
        var mockUpdateUserCommand =
            new UpdateUserCommand(1, "Test name", "Test surname");
        var mockUserDto = new UserDto
        {
            Id = 1,
            Name = "Test2"
        };
        _mockMediator.Setup(s => s.Send(It.IsAny<UpdateUserCommand>()
            , It.Is<CancellationToken>(x => x == default))).ReturnsAsync(mockUserDto);

        //Act
        var result = await _sut.PutAsync(mockUpdateUserCommand);

        //Assert
        var resultObject = (UpdateUserResponse)Assert.IsType<OkObjectResult>(result.Result).Value;
        Assert.NotNull(resultObject);
        Assert.Equal(mockUserDto.Id, resultObject.Id);
        Assert.Equal(mockUserDto.Name, resultObject.Name);
        _mockMediator.VerifyAll();
    }

    [Fact]
    public async Task Given_UserDelete_When_WithGivenUser_Then_DeleteUser()
    {
        //Arrange
        _mockMediator.Setup(s => s.Send(It.IsAny<DeleteUserCommand>()
            , It.Is<CancellationToken>(x => x == default)));

        //Act
        await _sut.DeleteAsync(1);

        //Assert
        _mockMediator.VerifyAll();
    }
}