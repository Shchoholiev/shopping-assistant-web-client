using System.ComponentModel.DataAnnotations;

namespace ShoppingAssistantWebClient.Web.Models.Input;

public class LoginInputModel
{
    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string? Email { get; set; }

    [Phone(ErrorMessage = "Invalid phone number")]
    public string? Phone { get; set; }

    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; }

    public bool IsEmailOrPhoneProvided => !string.IsNullOrEmpty(Email) || !string.IsNullOrEmpty(Phone);
}