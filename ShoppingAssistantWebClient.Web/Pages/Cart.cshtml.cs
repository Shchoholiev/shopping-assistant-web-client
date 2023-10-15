using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ShoppingAssistantWebClient.Web.Pages
{
    public class CartModel : PageModel
    {
        public List<Product> products = new List<Product> {
                new Product {Description = "HDMI cabel HDMI cabel HDMI cabel HDMI cabel HDMI cabel HDMI cabelHDMI cabel", Rating = 4.0, Price = 12},
                new Product {Description = "super mega hdmi cabel", Rating = 3.8, Price = 13.11},
                new Product {Description = "", Rating = 4.0}
        };
    
        public void OnGet()
        {

        }
    }

    public class Product {
        public string Description {get; set;}
        public double? Rating {get; set;}
        public double? Price {get; set;}
    }
}
