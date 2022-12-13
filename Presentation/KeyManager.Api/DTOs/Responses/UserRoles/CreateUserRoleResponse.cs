namespace KeyManager.Api.DTOs.Responses.UserRoles;

/// <summary>
///     Class for response of user role
/// </summary>
public class CreateUserRoleResponse
{
    /// <summary>
    ///     Id of user role
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