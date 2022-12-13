namespace KeyManager.Api.DTOs.Responses.Doors;

/// <summary>
///     Door response
/// </summary>
public class GetDoorResponse
{
    /// <summary>
    ///     Id of the door
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    ///     Name of the door
    /// </summary>
    public string Name { get; set; }
}