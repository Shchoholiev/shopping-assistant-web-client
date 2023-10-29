using Microsoft.AspNetCore.Components;
using ShoppingAssistantWebClient.Web.Models;

namespace ShoppingAssistantWebClient.Web.Shared
{
    public partial class NavMenu : ComponentBase
    {

            public List<Wishlist> Wishlists = new()
            {
                new Wishlist {Id = "0", Name = "Gift for Jessica", Type="product", CreateById="0"},
                new Wishlist {Id = "1", Name = "Secret Santa", Type="gift", CreateById="1"},
                new Wishlist  {Id = "2", Name = "Mark’s Birthday", Type="product", CreateById="2"}
            };

            protected override async Task OnInitializedAsync()
            {
                // Get data from Back-end
            }

    }

}
