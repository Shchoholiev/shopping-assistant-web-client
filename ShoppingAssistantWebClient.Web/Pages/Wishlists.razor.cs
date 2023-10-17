using Microsoft.AspNetCore.Components;
using ShoppingAssistantWebClient.Web.Models;

namespace ShoppingAssistantWebClient.Web.Pages;

public partial class Wishlists : ComponentBase
{
    public List<Wishlist> WishlistList = new()
    {
        new Wishlist {Id = "0", Name = "Chat1", Type="product", CreateById="0"},
        new Wishlist {Id = "1", Name = "Chat2", Type="gift", CreateById="1"},
        new Wishlist {Id = "2", Name = "Chat3", Type="product", CreateById="2"}
    };
}