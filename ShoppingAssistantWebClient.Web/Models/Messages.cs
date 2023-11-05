namespace ShoppingAssistantWebClient.Web.Models
{
    public class Messages
    {

        public required string Id { get; set; }

        public required string Text { get; set; }

        public required string Role { get; set; }

        public required string CreatedById { get; set; }
    }
}
