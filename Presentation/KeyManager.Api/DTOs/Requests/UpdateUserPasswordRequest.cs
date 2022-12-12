using System.ComponentModel.DataAnnotations;

namespace KeyManager.Api.DTOs.Requests;

public class UpdateUserPasswordRequest
{
    public UpdateUserPasswordRequest(string oldPassword, string newPassword)
    {
        OldPassword = oldPassword;
        NewPassword = newPassword;
    }

    [Required(ErrorMessage = "OldPassword is required.")]
    [DataType(DataType.Password)]
    public string OldPassword { get; }

    [DataType(DataType.Password)]
    [Required(ErrorMessage = "NewPassword is required.")]
    public string NewPassword { get; }
}