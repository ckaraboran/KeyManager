using KeyManager.Domain.Enums;

namespace KeyManager.Api.Security.Requirements;

/// <summary>
///     A requirement for a user to be in a specific role.
/// </summary>
public interface IAccessRequirement : IAuthorizationRequirement
{
    /// <summary>
    ///     Gets the roles required for access.
    /// </summary>
    /// <returns></returns>
    IEnumerable<KnownRoles> GetAllowedRoles();
}