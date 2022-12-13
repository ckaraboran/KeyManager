namespace KeyManager.Api.DTOs.Responses.Users;

/// <summary>
///     Create user response
/// </summary>
public class CreateUserResponse
{
    /// <summary>
    ///     Id of the created user
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    ///     Username of the created user
    /// </summary>
    public string Username { get; set; }

    /// <summary>
    ///     Name of the created user
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    ///     Surname of the created user
    /// </summary>
    public string Surname { get; set; }
}