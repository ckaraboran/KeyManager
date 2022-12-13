namespace KeyManager.Api.DTOs.Responses.Users;

/// <summary>
///     Update user response
/// </summary>
public class UpdateUserResponse
{
    /// <summary>
    ///     Id of the user
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    ///     Username of the user
    /// </summary>
    public string Username { get; set; }

    /// <summary>
    ///     Name of the user
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    ///     Surname of the user
    /// </summary>
    public string Surname { get; set; }
}