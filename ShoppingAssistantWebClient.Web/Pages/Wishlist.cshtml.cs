using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ShoppingAssistantWebClient.Web.Pages
{
    public class WishlistModel : PageModel
    {
        public List<Wishlist> wishlist = new List<Wishlist>{
                new Wishlist {Id = 0, Name = "Chat1", Type="product", Date="21.09.2023"},
                new Wishlist {Id = 1, Name = "Chat2", Type="gift", Date="29.09.2023"},
                new Wishlist {Id = 2, Name = "Chat3", Type="product", Date="02.10.2023"}
            };
        public void OnGet()
        {

        }

        public void OnPostDelete(int id) {
            var item = wishlist.FirstOrDefault(wishlist => wishlist.Id == id);
            wishlist.RemoveAt(id);
        }

        public IActionResult OnPostMoveToChat() {
            return RedirectToPage("Index");
        }
    }

    public class Wishlist {
        public int Id {get; set;}
        public string Name {get; set;}
        public string Type {get; set;}
        public string Date {get; set;}
    }
}
