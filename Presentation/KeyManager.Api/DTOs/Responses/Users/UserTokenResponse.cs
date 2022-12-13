namespace KeyManager.Api.DTOs.Responses.Users;

/// <summary>
///     Token response
/// </summary>
public class UserTokenResponse
{
    /// <summary>
    ///     Constructor for the token response
    /// </summary>
    /// <param name="token"></param>
    public UserTokenResponse(string token)
    {
        Token = token;
    }

    /// <summary>
    ///     Token
    /// </summary>
    public string Token { get; }
}