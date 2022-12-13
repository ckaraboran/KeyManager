namespace KeyManager.Api.DTOs.Responses.UserRoles;

/// <summary>
///     Get user roles response
/// </summary>
public class GetUserRoleResponse
{
    /// <summary>
    ///     Id
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    ///     User id
    /// </summary>
    public long UserId { get; set; }

    /// <summary>
    ///     User name
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    ///     Role id
    /// </summary>
    public long RoleId { get; set; }

    /// <summary>
    ///     Role name
    /// </summary>
    public string RoleName { get; set; }
}