namespace KeyManager.Api.DTOs.Responses.UserRoles;

/// <summary>
///     User role response
/// </summary>
public class UpdateUserRoleResponse
{
    /// <summary>
    ///     Id of the user role
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    ///     User Id
    /// </summary>
    public long UserId { get; set; }

    /// <summary>
    ///     Role Id
    /// </summary>
    public long RoleId { get; set; }
}