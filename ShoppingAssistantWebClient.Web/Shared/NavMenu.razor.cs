using Microsoft.AspNetCore.Components;
using ShoppingAssistantWebClient.Web.Models;
using GraphQL;
using Newtonsoft.Json;
using ShoppingAssistantWebClient.Web.Network;

namespace ShoppingAssistantWebClient.Web.Shared
{
    public partial class NavMenu : ComponentBase
    {

        [Inject]
        private ApiClient _apiClient { get; set; }
        public List<Wishlist> Wishlists { get; set; }
        public bool isLoading = true;

        protected override async Task OnInitializedAsync()
        {
            await LoadMenus();
        }
        private async Task LoadMenus()
        {
            isLoading = true;
            var pageNumber = 1;
            var request = new GraphQLRequest
            {
                Query = @"query PersonalWishlistsPage( $pageNumber: Int!, $pageSize: Int!) {
                            personalWishlistsPage(pageNumber: $pageNumber, pageSize: $pageSize) {
                                items {
                                    id
                                    name
                                }
                            }
                        }",

                Variables = new
                {
                    pageNumber,
                    pageSize = 40,
                }
            };

            var response = await _apiClient.QueryAsync(request);
            var responseData = response.Data;
            var jsonCategoriesResponse = JsonConvert.SerializeObject(responseData.personalWishlistsPage.items);
            this.Wishlists = JsonConvert.DeserializeObject<List<Wishlist>>(jsonCategoriesResponse);
            isLoading = false;
        }

        protected async Task DeleteWish(string wishlistId)
        {
            var request = new GraphQLRequest
            {
                Query = @"mutation DeletePersonalWishlist($wishlistId: String!) {
                            deletePersonalWishlist(wishlistId: $wishlistId) {
                                id
                            }
                        }
                        ",

                Variables = new
                {
                    wishlistId
                }
            };

            var response = await _apiClient.QueryAsync(request);
            await LoadMenus();
        }

    }

}