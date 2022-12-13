namespace KeyManager.Api.DTOs.Responses.Doors;

/// <summary>
///     Create door response
/// </summary>
public class CreateDoorResponse
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