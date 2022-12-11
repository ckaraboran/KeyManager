using System.Linq;
using System.Security.Claims;
using KeyManager.Api.Security.Requirements;
using KeyManager.Application.Commands.UserRoles;
using MediatR;

namespace KeyManager.Api.Security.Handlers;

public class ManageUsersAuthorizationHandler : AuthorizationHandler<ManageUsersRequirement>
{
    private readonly ISender _mediator;

    public ManageUsersAuthorizationHandler(ISender mediator)
    {
        _mediator = mediator;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
        ManageUsersRequirement requirement)
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