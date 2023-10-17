using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShoppingAssistantWebClient.Web.Models;

namespace ShoppingAssistantWebClient.Web.Pages
{
    public class WishlistModel : PageModel
    {
        public List<Models.Wishlist> wishlist = new List<Models.Wishlist>{
                new Models.Wishlist {Id = "0", Name = "Chat1", Type="product", CreateById="0"},
                new Models.Wishlist {Id = "1", Name = "Chat2", Type="gift", CreateById="1"},
                new Models.Wishlist {Id = "2", Name = "Chat3", Type="product", CreateById="2"}
            };
        public void OnGet()
        {

        }

        public void OnPostDelete(string id) {
            var item = wishlist.FirstOrDefault(wishlist => wishlist.Id == id);
            wishlist.RemoveAt(Int32.Parse(id));
        }

        public IActionResult OnPostMoveToChat() {
            return RedirectToPage("Index");
        }
    }
}
