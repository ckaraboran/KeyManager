using System.Threading;
using KeyManager.Api.DTOs.Responses.Permissions;
using KeyManager.Application.Commands.Permissions;
using KeyManager.Application.Queries.Permissions;
using MediatR;

namespace KeyManager.Api.Tests.Controllers;

public class PermissionControllerTests
{
    private readonly Mock<ISender> _mockMediator;
    private readonly PermissionController _sut;

    public PermissionControllerTests()
    {
        _mockMediator = new Mock<ISender>();
        var myProfile = new AutoMapperProfile();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        var mapper = new Mapper(configuration);
        _sut = new PermissionController(_mockMediator.Object, mapper);
    }

    [Fact]
    public async Task Permission_GetAsync_ShouldReturnAllPermissions()
    {
        //Arrange
        var mockPermissionDto = new List<PermissionWithNamesDto>
        {
            new() { Id = 1, UserId = 1, UserName = "Username 1", DoorId = 1, DoorName = "Door name 1" },
            new() { Id = 1, UserId = 2, UserName = "Username 2", DoorId = 2, DoorName = "Door name 2" }
        };
        _mockMediator.Setup(s => s.Send(It.IsAny<GetPermissionsQuery>(), It.Is<CancellationToken>(x => x == default)))
            .ReturnsAsync(mockPermissionDto);

        //Act
        var result = await _sut.GetAsync();

        //Assert
        var resultObject = (List<GetPermissionResponse>)Assert.IsType<OkObjectResult>(result.Result).Value;
        Assert.NotNull(resultObject);
        Assert.Equal(mockPermissionDto[0].Id, resultObject[0].Id);
        Assert.Equal(mockPermissionDto[0].UserId, resultObject[0].UserId);
        Assert.Equal(mockPermissionDto[0].UserName, resultObject[0].UserName);
        Assert.Equal(mockPermissionDto[0].DoorId, resultObject[0].DoorId);
        Assert.Equal(mockPermissionDto[0].DoorName, resultObject[0].DoorName);
        Assert.Equal(mockPermissionDto[1].Id, resultObject[1].Id);
        Assert.Equal(mockPermissionDto[1].UserId, resultObject[1].UserId);
        Assert.Equal(mockPermissionDto[1].UserName, resultObject[1].UserName);
        Assert.Equal(mockPermissionDto[1].DoorId, resultObject[1].DoorId);
        Assert.Equal(mockPermissionDto[1].DoorName, resultObject[1].DoorName);
        _mockMediator.VerifyAll();
    }

    [Fact]
    public async Task Permission_PostAsync_WithGivenPermission_ShouldAddPermission()
    {
        //Arrange
        var mockPermissionDto = new PermissionDto
        {
            Id = 1, UserId = 1, DoorId = 1
        };
        _mockMediator.Setup(s => s.Send(It.IsAny<CreatePermissionCommand>()
            , It.Is<CancellationToken>(x => x == default))).ReturnsAsync(mockPermissionDto);

        //Act
        var result =
            await _sut.PostAsync(new CreatePermissionCommand(mockPermissionDto.UserId, mockPermissionDto.DoorId));

        //Assert
        var resultObject = (CreatePermissionResponse)Assert.IsType<CreatedResult>(result.Result).Value;
        Assert.NotNull(resultObject);
        Assert.Equal(mockPermissionDto.Id, resultObject.Id);
        Assert.Equal(mockPermissionDto.UserId, resultObject.UserId);
        Assert.Equal(mockPermissionDto.DoorId, resultObject.DoorId);
        _mockMediator.VerifyAll();
    }

    [Fact]
    public async Task Permission_DeleteAsync_WithGivenPermission_ShouldDeletePermission()
    {
        //Arrange
        _mockMediator.Setup(s => s.Send(It.IsAny<DeletePermissionCommand>()
            , It.Is<CancellationToken>(x => x == default)));

        //Act
        await _sut.DeleteAsync(1);

        //Assert
        _mockMediator.VerifyAll();
    }
}