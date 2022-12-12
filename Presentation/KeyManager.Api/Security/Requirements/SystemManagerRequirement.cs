using KeyManager.Domain.Enums;

namespace KeyManager.Api.Security.Requirements;

public class SystemManagerRequirement : IAccessRequirement
{
    private static List<KnownRoles> AllowedRoles { get; } = new()
    {
        KnownRoles.OfficeManager
    };

    public IEnumerable<KnownRoles> GetAllowedRoles()
    {
        return AllowedRoles;
    }
}