using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShoppingAssistantWebClient.Web.Network;

namespace ShoppingAssistantWebClient.Web.Pages;
public class ChatModel : PageModel
{
    private readonly ILogger<ChatModel> _logger;

    private readonly AuthenticationService _authenticationService;

    public ChatModel(ILogger<ChatModel> logger, AuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
        _logger = logger;
    }

    public void OnGet()
    {
    }
}
