using KeyManager.Domain.Enums;

namespace KeyManager.Api.Security.Requirements;

public interface IAccessRequirement : IAuthorizationRequirement
{
    IEnumerable<KnownRoles> GetAllowedRoles();
}