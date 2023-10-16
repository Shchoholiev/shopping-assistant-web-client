using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShoppingAssistantWebClient.Web.Network;

namespace ShoppingAssistantWebClient.Web.Pages;
public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    private readonly AuthenticationService _authenticationService;

    public IndexModel(ILogger<IndexModel> logger, AuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
        _logger = logger;
    }

    public void OnGet()
    {
    }
}
