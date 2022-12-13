namespace KeyManager.Api.DTOs.Responses.Roles;

/// <summary>
///     User role response
/// </summary>
public class UpdateRoleResponse
{
    /// <summary>
    ///     Id of the role
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    ///     Name of the role
    /// </summary>
    public string Name { get; set; }
}