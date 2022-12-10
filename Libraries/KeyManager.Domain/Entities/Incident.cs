using System.ComponentModel.DataAnnotations.Schema;

namespace KeyManager.Domain.Entities;

public class Incident : BaseEntity
{
    [ForeignKey(nameof(User))]
    public long UserId { get; set; }
    public User User { get; set; }
    [ForeignKey(nameof(Door))]
    public long DoorId { get; set; }
    public Door Door { get; set; }
    public DateTime EventDate { get; set; }
}