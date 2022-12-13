namespace KeyManager.Api.Tests.Security;

public class AuthorizationHandlerTests
{
    private readonly AuthorizationHandler _authorizationHandler;

    public AuthorizationHandlerTests()
    {
        _authorizationHandler = new AuthorizationHandler();
    }

    [Fact]
    public async Task Given_AuthorizationRequirement_When_UserAuthorized_Then_ShouldBeSuccessful()
    {
        //Arrange    
        var requirements = new[] { new AuthorizationRequirement() };
        var user = new ClaimsPrincipal(
            new ClaimsIdentity(
                new[]
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, "test")
                },
                JwtBearerDefaults.AuthenticationScheme)
        );
        var context = new AuthorizationHandlerContext(requirements, user, null);

        //Act
        await _authorizationHandler.HandleAsync(context);

        //Assert
        Assert.True(context.HasSucceeded);
    }


    [Fact]
    public async Task Given_AuthorizationRequirement_When_UserNotKnown_Then_ShouldFail()
    {
        //Arrange    
        var requirements = new[] { new AuthorizationRequirement() };
        var user = new ClaimsPrincipal();
        var context = new AuthorizationHandlerContext(requirements, user, null);

        //Act
        await _authorizationHandler.HandleAsync(context);

        //Assert
        Assert.True(context.HasFailed);
    }
}