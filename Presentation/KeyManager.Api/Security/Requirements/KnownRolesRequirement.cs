using KeyManager.Domain.Enums;

namespace KeyManager.Api.Security.Requirements;

/// <summary>
///     Requirement for the user to be known to the system.
/// </summary>
public class KnownRolesRequirement : IAccessRequirement
{
    private static List<KnownRoles> AllowedRoles { get; } = new()
    {
        KnownRoles.OfficeManager,
        KnownRoles.Director,
        KnownRoles.OfficeUser
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