namespace KeyManager.Api.DTOs.Responses.Users;

public class UserTokenResponse
{
    public UserTokenResponse(string token)
    {
        Token = token;
    }

    public string Token { get; }
}