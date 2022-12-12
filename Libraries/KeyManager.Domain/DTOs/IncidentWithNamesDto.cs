namespace KeyManager.Domain.DTOs;

public class IncidentWithNamesDto
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public string UserName { get; set; }
    public long DoorId { get; set; }
    public string DoorName { get; set; }
    public DateTimeOffset IncidentDate { get; set; }
}