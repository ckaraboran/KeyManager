namespace KeyManager.Api.DTOs.Responses.Users;

/// <summary>
///     Get user response
/// </summary>
public class GetUserResponse
{
    /// <summary>
    ///     Id
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    ///     Username
    /// </summary>
    public string Username { get; set; }

    /// <summary>
    ///     Name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    ///     Surname
    /// </summary>
    public string Surname { get; set; }
}