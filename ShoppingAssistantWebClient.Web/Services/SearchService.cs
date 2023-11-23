
namespace ShoppingAssistantWebClient.Web.Services;

public class SearchService
{
    public List<String> Products { get; set; }

    public string FirstMessage { get; set; }

    public void SetProducts(List<String> products) {
        Products = products;
    }

    public void SetFirstMessage(string message) {
        FirstMessage = message;
    }
}