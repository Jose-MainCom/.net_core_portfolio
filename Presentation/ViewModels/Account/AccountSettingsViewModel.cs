using System.ComponentModel.DataAnnotations;

namespace Presentation.ViewModels.Account;

public class AccountSettingsViewModel
{
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Enter a valid email address.")]
    public string Email { get; set; } = string.Empty;

    [DataType(DataType.Password)]
    public string CurrentPassword { get; set; } = string.Empty;

    [DataType(DataType.Password)]
    [MinLength(6, ErrorMessage = "The new password must be at least 6 characters.")]
    public string NewPassword { get; set; } = string.Empty;

    [DataType(DataType.Password)]
    [Compare(nameof(NewPassword), ErrorMessage = "The passwords do not match.")]
    public string ConfirmNewPassword { get; set; } = string.Empty;
}