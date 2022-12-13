using System.ComponentModel.DataAnnotations;

namespace KeyManager.Api.DTOs.Requests;

/// <summary>
///     Request to update user password
/// </summary>
public class UpdateUserPasswordRequest
{
    /// <summary>
    ///     Constructor for UpdateUserPasswordRequest
    /// </summary>
    /// <param name="oldPassword"></param>
    /// <param name="newPassword"></param>
    public UpdateUserPasswordRequest(string oldPassword, string newPassword)
    {
        OldPassword = oldPassword;
        NewPassword = newPassword;
    }

    /// <summary>
    ///     Old password
    /// </summary>
    [Required(ErrorMessage = "OldPassword is required.")]
    [DataType(DataType.Password)]
    public string OldPassword { get; }

    /// <summary>
    ///     New password
    /// </summary>
    [DataType(DataType.Password)]
    [Required(ErrorMessage = "NewPassword is required.")]
    public string NewPassword { get; }
}