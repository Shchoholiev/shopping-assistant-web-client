using ShoppingAssistantWebClient.Web.Models;

namespace ShoppingAssistantWebClient.Web.Models
{
    public class User
    {
        public string Id { get; set; }

        public string GuestId { get; set; }

        public List<Role>? Roles { get; set; } = new List<Role>();

        public string Email { get; set; }

        public string Phone { get; set; }
    }
}