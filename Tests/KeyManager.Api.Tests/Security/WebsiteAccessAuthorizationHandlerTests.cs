using KeyManager.Api.Security.Handlers;
using KeyManager.Api.Security.Requirements;
using KeyManager.Application.Commands.UserRoles;
using MediatR;

namespace KeyManager.Api.Tests.Security;

public class WebsiteAccessAuthorizationHandlerTests
{
    private readonly WebsiteAccessAuthorizationHandler _authorizationHandler;
    private readonly Mock<ISender> _mockMediator;

    public WebsiteAccessAuthorizationHandlerTests()
    {
        _mockMediator = new Mock<ISender>();
        _authorizationHandler = new WebsiteAccessAuthorizationHandler(_mockMediator.Object);
    }

    [Fact]
    public async Task Given_SystemManagerRequirement_When_UserAuthorized_ThenShouldBeSuccessful()
    {
        //Arrange    
        var requirements = new[] { new SystemManagerRequirement() };
        var user = new ClaimsPrincipal(
            new ClaimsIdentity(
                new[]
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, "test")
                },
                JwtBearerDefaults.AuthenticationScheme)
        );
        var context = new AuthorizationHandlerContext(requirements, user, null);
        _mockMediator.Setup(x => x.Send(It.IsAny<CheckUserForRoleCommand>(), default))
            .ReturnsAsync(true);
        //Act
        await _authorizationHandler.HandleAsync(context);

        //Assert
        Assert.True(context.HasSucceeded);
    }

    [Fact]
    public async Task Given_SystemManagerRequirement_When_UserCannotManageUsers_ThenShouldFail()
    {
        //Arrange    
        var requirements = new[] { new SystemManagerRequirement() };
        var user = new ClaimsPrincipal(
            new ClaimsIdentity(
                new[]
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, "test")
                },
                JwtBearerDefaults.AuthenticationScheme)
        );
        var context = new AuthorizationHandlerContext(requirements, user, null);
        _mockMediator.Setup(x => x.Send(It.IsAny<CheckUserForRoleCommand>(), default))
            .ReturnsAsync(false);
        //Act
        await _authorizationHandler.HandleAsync(context);

        //Assert
        Assert.True(context.HasFailed);
    }

    [Fact]
    public async Task Given_SystemManagerRequirement_When_UserNotKnown_ThenShouldFail()
    {
        //Arrange    
        var requirements = new[] { new SystemManagerRequirement() };
        var user = new ClaimsPrincipal();
        var context = new AuthorizationHandlerContext(requirements, user, null);
        _mockMediator.Setup(x => x.Send(It.IsAny<CheckUserForRoleCommand>(), default))
            .ReturnsAsync(true);

        //Act
        await _authorizationHandler.HandleAsync(context);

        //Assert
        Assert.True(context.HasFailed);
    }

    [Fact]
    public async Task Given_KnownRolesRequirement_When_UserAuthorized_ThenShouldBeSuccessful()
    {
        //Arrange    
        var requirements = new[] { new KnownRolesRequirement() };
        var user = new ClaimsPrincipal(
            new ClaimsIdentity(
                new[]
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, "test")
                },
                JwtBearerDefaults.AuthenticationScheme)
        );
        var context = new AuthorizationHandlerContext(requirements, user, null);
        _mockMediator.Setup(x => x.Send(It.IsAny<CheckUserForRoleCommand>(), default))
            .ReturnsAsync(true);
        //Act
        await _authorizationHandler.HandleAsync(context);

        //Assert
        Assert.True(context.HasSucceeded);
    }

    [Fact]
    public async Task Given_KnownRolesRequirement_When_UserCannotManageUsers_ThenShouldFail()
    {
        //Arrange    
        var requirements = new[] { new KnownRolesRequirement() };
        var user = new ClaimsPrincipal(
            new ClaimsIdentity(
                new[]
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, "test")
                },
                JwtBearerDefaults.AuthenticationScheme)
        );
        var context = new AuthorizationHandlerContext(requirements, user, null);
        _mockMediator.Setup(x => x.Send(It.IsAny<CheckUserForRoleCommand>(), default))
            .ReturnsAsync(false);
        //Act
        await _authorizationHandler.HandleAsync(context);

        //Assert
        Assert.True(context.HasFailed);
    }

    [Fact]
    public async Task Given_KnownRolesRequirement_When_UserNotKnown_ThenShouldFail()
    {
        //Arrange    
        var requirements = new[] { new KnownRolesRequirement() };
        var user = new ClaimsPrincipal();
        var context = new AuthorizationHandlerContext(requirements, user, null);
        _mockMediator.Setup(x => x.Send(It.IsAny<CheckUserForRoleCommand>(), default))
            .ReturnsAsync(true);

        //Act
        await _authorizationHandler.HandleAsync(context);

        //Assert
        Assert.True(context.HasFailed);
    }
}