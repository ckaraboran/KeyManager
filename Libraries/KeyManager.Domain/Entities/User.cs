using System.ComponentModel.DataAnnotations;

namespace KeyManager.Domain.Entities;

public class User : BaseEntity
{
    public long EmployeeId { get; set; }

    [Required] public string Name { get; set; }

    [Required] public string Surname { get; set; }
}