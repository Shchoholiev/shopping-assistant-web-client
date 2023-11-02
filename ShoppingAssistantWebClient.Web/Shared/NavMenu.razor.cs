using Microsoft.AspNetCore.Components;
using ShoppingAssistantWebClient.Web.Models;
using GraphQL;
using Newtonsoft.Json;
using ShoppingAssistantWebClient.Web.Models;
using ShoppingAssistantWebClient.Web.Network;

namespace ShoppingAssistantWebClient.Web.Shared
{
    public partial class NavMenu : ComponentBase
    {

        public List<Wishlist> Wishlists { get; set; }

        private readonly ApiClient _apiClient;

        public NavMenu()
        {
        }

        public NavMenu(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }
        public async Task OnGetAsync()
        {
            var request = new GraphQLRequest
            {
                Query = @"query PersonalWishlistsPage {
                            personalWishlistsPage(pageNumber: $pageNumber, pageSize: $pageSize) {
                                items {
                                    id
                                    name
                                }
                            }
                        }
                        ",

                Variables = new
                {
                    pageNumber = 1,
                    pageSize = 10,
                }
            };
            var response = await _apiClient.QueryAsync(request);
            var jsonCategoriesResponse = JsonConvert.SerializeObject(response.Data.personalWishlistsPage.items);
            this.Wishlists = JsonConvert.DeserializeObject<List<Wishlist>>(jsonCategoriesResponse);
        }
    }
}
