using KeyManager.Domain.Enums;

namespace KeyManager.Api.Security.Requirements;

public class UserManagerRequirement : IAccessRequirement
{
    private static List<KnownRoles> AllowedRoles { get; } = new()
    {
        KnownRoles.OfficeManager,
        KnownRoles.Director
    };

    public IEnumerable<KnownRoles> GetAllowedRoles()
    {
        return AllowedRoles;
    }
}