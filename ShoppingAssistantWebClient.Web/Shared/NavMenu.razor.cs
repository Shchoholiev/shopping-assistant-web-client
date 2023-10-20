
using Microsoft.AspNetCore.Components;
using ShoppingAssistantWebClient.Web.Models;

namespace ShoppingAssistantWebClient.Web.Pages;

public partial class Wishlists : ComponentBase
{
private List<Wishlist> wishlist;

        protected override async Task OnInitializedAsync()
        {

            wishlist =  new List<Models.Wishlist>
            {
                new Models.Wishlist {Id = "0", Name = "Gift for Jessica", Type="product", CreateById="0"},
                new Models.Wishlist  {Id = "1", Name = "Secret Santa", Type="gift", CreateById="1"},
                new Models.Wishlist  {Id = "2", Name = "Mark’s Birthday", Type="product", CreateById="2"},
                new Models.Wishlist  {Id = "3", Name = "Garden tools", Type="product", CreateById="2"},
                new Models.Wishlist  {Id = "4", Name = "Phone charger ", Type="product", CreateById="2"},
                new Models.Wishlist  {Id = "5", Name = "Garden tools", Type="product", CreateById="2"}
            };

        }
}
