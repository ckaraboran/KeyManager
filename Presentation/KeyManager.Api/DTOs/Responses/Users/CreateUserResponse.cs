namespace KeyManager.Api.DTOs.Responses.Users;

public class CreateUserResponse
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
}