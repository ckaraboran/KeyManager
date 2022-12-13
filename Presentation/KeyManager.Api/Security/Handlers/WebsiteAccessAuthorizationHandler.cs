using System.Linq;
using System.Security.Claims;
using KeyManager.Api.Security.Requirements;
using KeyManager.Application.Commands.UserRoles;
using MediatR;

namespace KeyManager.Api.Security.Handlers;

/// <summary>
///     Authorization handler for the interface <see cref="IAccessRequirement" />.
/// </summary>
public class WebsiteAccessAuthorizationHandler : AuthorizationHandler<IAccessRequirement>
{
    private readonly ISender _mediator;

    /// <summary>
    ///     Constructor.
    /// </summary>
    /// <param name="mediator"></param>
    public WebsiteAccessAuthorizationHandler(ISender mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    ///     Handles the authorization.
    /// </summary>
    /// <param name="context"></param>
    /// <param name="requirement"></param>
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
        IAccessRequirement requirement)
    {
        if (context.User.Identity is not { IsAuthenticated: true })
        {
            context.Fail();
            return;
        }

        var user = context.User.Claims.First(x => x.Type == ClaimTypes.Name).Value;
        var hasClaim = false;
        foreach (var allowedRole in requirement.GetAllowedRoles())
        {
            var hasClaimOnSingleRole = await _mediator
                .Send(new CheckUserForRoleCommand(user, allowedRole.ToString()));
            if (hasClaimOnSingleRole)
            {
                hasClaim = true;
                break;
            }
        }

        if (!hasClaim)
        {
            context.Fail();
            AuthorizationResult.Failed();
        }

        context.Succeed(requirement);
    }
}