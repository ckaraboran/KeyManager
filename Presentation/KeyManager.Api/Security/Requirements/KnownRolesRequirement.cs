using KeyManager.Domain.Enums;

namespace KeyManager.Api.Security.Requirements;

public class KnownRolesRequirement : IAccessRequirement
{
    private static List<KnownRoles> AllowedRoles { get; } = new()
    {
        KnownRoles.OfficeManager,
        KnownRoles.Director,
        KnownRoles.OfficeUser
    };

    public IEnumerable<KnownRoles> GetAllowedRoles()
    {
        return AllowedRoles;
    }
}