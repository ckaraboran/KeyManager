namespace KeyManager.Api.DTOs.Responses.Incident;

/// <summary>
///     Get Incident Response
/// </summary>
public class GetIncidentResponse
{
    /// <summary>
    ///     Incident Id
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    ///     User Id
    /// </summary>
    public long UserId { get; set; }

    /// <summary>
    ///     User Name
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    ///     Door Id
    /// </summary>
    public long DoorId { get; set; }

    /// <summary>
    ///     Door Name
    /// </summary>
    public string DoorName { get; set; }

    /// <summary>
    ///     Incident Date
    /// </summary>
    public DateTimeOffset IncidentDate { get; set; }
}