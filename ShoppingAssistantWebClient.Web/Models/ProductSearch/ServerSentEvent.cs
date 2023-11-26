using ShoppingAssistantWebClient.Web.Models.Enums;

namespace ShoppingAssistantWebClient.Web.Models.ProductSearch;

public class ServerSentEvent
{
    public SearchEventType Event { get; set; }

    public string Data { get; set; }
}
