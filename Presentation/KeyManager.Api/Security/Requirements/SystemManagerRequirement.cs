using KeyManager.Domain.Enums;

namespace KeyManager.Api.Security.Requirements;

/// <summary>
///     Represents a requirement for a system manager
/// </summary>
public class SystemManagerRequirement : IAccessRequirement
{
    private static List<KnownRoles> AllowedRoles { get; } = new()
    {
        KnownRoles.OfficeManager
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