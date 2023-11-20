
namespace ShoppingAssistantWebClient.Web.Services;

public class SearchService
{
    public List<String> Products { get; set; }

    public string firstMassage { get; set; }

    public void SetProducts(List<String> products) {
        Products = products;
    }

    public void SetFirstMassage(string massage) {
        firstMassage = massage;
    }
}