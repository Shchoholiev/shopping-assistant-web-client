using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShoppingAssistantWebClient.Web.Models;

namespace ShoppingAssistantWebClient.Web.Pages
{
    public class CartModel : PageModel
    {
        public List<ProductModel> products = new List<ProductModel> {
                new ProductModel {Id = "0", Url = "some link", Name = "HDMI", Description = "super mega hdmi cabel", Rating = 3.8, ImagesUrls = new string[] {"link"}, WasOpened = false, WishlistId = "0"},
                new ProductModel {Id = "1", Url = "some link", Name = "HDMI", Description = "super mega hdmi cabel", Rating = 3.8, ImagesUrls = new string[] {"link"}, WasOpened = false, WishlistId = "1"},
                new ProductModel {Id = "2", Url = "some link", Name = "HDMI", Description = "super mega hdmi cabel", Rating = 3.8, ImagesUrls = new string[] {"link"}, WasOpened = false, WishlistId = "2"}
        };
    
        public void OnGet()
        {

        }
    }
}
