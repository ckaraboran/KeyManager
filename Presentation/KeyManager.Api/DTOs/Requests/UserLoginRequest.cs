namespace KeyManager.Api.DTOs.Requests;

/// <summary>
///     User login request
/// </summary>
public class UserLoginRequest
{
    /// <summary>
    ///     User name
    /// </summary>
    public string Username { get; set; }

    /// <summary>
    ///     Password
    /// </summary>
    public string Password { get; set; }
}