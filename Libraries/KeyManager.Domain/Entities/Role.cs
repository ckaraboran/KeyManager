using System.ComponentModel.DataAnnotations;

namespace KeyManager.Domain.Entities;

public class Role: BaseEntity
{
    [Required]
    public string Name { get; set; }
}