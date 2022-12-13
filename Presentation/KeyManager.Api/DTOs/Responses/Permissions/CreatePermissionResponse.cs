namespace KeyManager.Api.DTOs.Responses.Permissions;

/// <summary>
///     Create permission response
/// </summary>
public class CreatePermissionResponse
{
    /// <summary>
    ///     Id of the created permission
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    ///     User id
    /// </summary>
    public long UserId { get; set; }

    /// <summary>
    ///     Door Id
    /// </summary>
    public long DoorId { get; set; }
}