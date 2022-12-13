namespace KeyManager.Api.DTOs.Responses.Doors;

/// <summary>
///     Update door response
/// </summary>
public class UpdateDoorResponse
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