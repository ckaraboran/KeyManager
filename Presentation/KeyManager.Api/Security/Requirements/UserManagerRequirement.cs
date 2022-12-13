using KeyManager.Domain.Enums;

namespace KeyManager.Api.Security.Requirements;

/// <summary>
///     Requirement for the user to be a user manager in the system
/// </summary>
public class UserManagerRequirement : IAccessRequirement
{
    private static List<KnownRoles> AllowedRoles { get; } = new()
    {
        KnownRoles.OfficeManager,
        KnownRoles.Director
    };

    /// <summary>
    ///     Gets the roles required for access.
    /// </summary>
    /// <returns></returns>
    public IEnumerable<KnownRoles> GetAllowedRoles()
    {
        return AllowedRoles;
    }
}