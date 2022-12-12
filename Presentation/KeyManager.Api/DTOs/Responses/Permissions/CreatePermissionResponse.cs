namespace KeyManager.Api.DTOs.Responses.Permissions;

public class CreatePermissionResponse
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public long DoorId { get; set; }
}