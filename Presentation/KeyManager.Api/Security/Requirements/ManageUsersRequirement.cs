using KeyManager.Domain.Enums;

namespace KeyManager.Api.Security.Requirements;

public class ManageUsersRequirement : IAuthorizationRequirement
{
    private static readonly List<KnownRoles> AllowedRoles = new()
    {
        KnownRoles.OfficeManager,
        KnownRoles.Director
    };

    public IEnumerable<KnownRoles> GetAllowedRoles()
    {
        return AllowedRoles;
    }
}