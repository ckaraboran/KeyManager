namespace KeyManager.Domain.Entities;

public class Incident : BaseEntity
{
    public long UserId { get; set; }
    public User User { get; set; }
    public long DoorId { get; set; }
    public Door Door { get; set; }
    public DateTime EventDate { get; set; }
}