namespace KeyManager.Api.DTOs.Responses.Permissions;

/// <summary>
///     Get permission response
/// </summary>
public class GetPermissionResponse
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
    ///     Door Id
    /// </summary>
    public long DoorId { get; set; }

    /// <summary>
    ///     Door name
    /// </summary>
    public string DoorName { get; set; }
}