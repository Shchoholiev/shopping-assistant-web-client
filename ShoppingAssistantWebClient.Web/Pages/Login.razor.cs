using Microsoft.AspNetCore.Components;
using ShoppingAssistantWebClient.Web.Network;
using ShoppingAssistantWebClient.Web.Models.Input;


namespace ShoppingAssistantWebClient.Web.Pages;

public partial class Login : ComponentBase 
{

    [Inject]
    NavigationManager NavigationManager { get; set; }

    [Inject]
    private AuthenticationService _authenticationService { get; set; }

    private string errorMessage = "";


    private void RedirectToNewChat() {
        var url = $"/";
        NavigationManager.NavigateTo(url);
    }

    public async Task LoginUser(LoginInputModel login) {
        if (login.IsEmailOrPhoneProvided)
        {
            try
            {
                await _authenticationService.LoginAsync(login);
                RedirectToNewChat();
            }
            catch (Exception ex)
            {
                errorMessage = "Login failed. Please try again.";
            }
        }
        else
        {
            errorMessage = "Please provide an email or phone number.";
        }
    }
}
