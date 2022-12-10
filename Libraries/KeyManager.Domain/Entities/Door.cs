using System.ComponentModel.DataAnnotations;

namespace KeyManager.Domain.Entities;

public class Door : BaseEntity
{
    [Required]
    public string Name { get; set; }
}