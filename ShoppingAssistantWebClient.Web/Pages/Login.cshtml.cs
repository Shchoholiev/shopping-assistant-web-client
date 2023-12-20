using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShoppingAssistantWebClient.Web.Models.Input;
using ShoppingAssistantWebClient.Web.Network;

namespace ShoppingAssistantWebClient.Web.Pages;

public class LoginModel : PageModel
{
    [BindProperty]
    public LoginInputModel Input { get; set; }

    public string ErrorMessage { get; set; }

    public AuthenticationService AuthenticationService { get; set; }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        if (Input.IsEmailOrPhoneProvided)
        {
            try
            {
                await AuthenticationService.LoginAsync(Input);
                return RedirectToPage("/");
            }
            catch (Exception ex)
            {
                ErrorMessage = "Login failed. Please try again.";
                return Page();
            }
        }
        else
        {
            ErrorMessage = "Please provide an email or phone number.";
            return Page();
        }
    }
}
