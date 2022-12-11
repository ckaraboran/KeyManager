namespace KeyManager.Application.Commands.Roles;

public class UpdateRoleCommand : IRequest<RoleDto>
{
    public UpdateRoleCommand(long id, string name)
    {
        Id = id;
        Name = name;
    }

    [Required(ErrorMessage = "Id is required.")]
    public long Id { get; }

    [Required(ErrorMessage = "Name is required.")]
    public string Name { get; }
}