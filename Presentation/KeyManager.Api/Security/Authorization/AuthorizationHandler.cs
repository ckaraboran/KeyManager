namespace KeyManager.Api.Security.Authorization;

/// <summary>
///     Authorization handler for the <see cref="AuthorizationRequirement" />.
/// </summary>
public sealed class AuthorizationHandler : AuthorizationHandler<AuthorizationRequirement>
{
    /// <summary>
    ///     Handles the authorization requirement.
    /// </summary>
    /// <param name="context"></param>
    /// <param name="requirement"></param>
    /// <returns></returns>
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
        AuthorizationRequirement requirement)
    {
        if (context.User.Identity is not { IsAuthenticated: true })
        {
            context.Fail();
            return Task.CompletedTask;
        }

        context.Succeed(requirement);
        return Task.CompletedTask;
    }
}