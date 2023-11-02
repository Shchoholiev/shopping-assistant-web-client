using Microsoft.AspNetCore.Components;

namespace ShoppingAssistantWebClient.Web.Pages;

public partial class Chat : ComponentBase
{
    [Inject]
    public ILogger<Chat> Logger { get; set; }

    protected override async Task OnInitializedAsync()
    {
        // Get data from Back-end
    }
}
