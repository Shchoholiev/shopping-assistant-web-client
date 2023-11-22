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

        public int pageSize { get; set; }
        public int currentPage { get; set; }

        protected override async Task OnInitializedAsync()
        {
            pageSize = 200;
            currentPage = 1;
            Wishlists = new List<Wishlist>();
            await LoadMenus(currentPage, pageSize);
        
        }
        
        public async Task LoadMenus(int pageNumber,  int pageSize )
        {
            try{
                    isLoading = true;
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
                        pageSize,
                    }
                };

                var response = await _apiClient.QueryAsync(request);
                var responseData = response.Data;
                var jsonCategoriesResponse = JsonConvert.SerializeObject(responseData.personalWishlistsPage.items);
                this.Wishlists = JsonConvert.DeserializeObject<List<Wishlist>>(jsonCategoriesResponse);
                Wishlists.Reverse();
                isLoading = false;
                StateHasChanged();

            }catch(Exception ex){
                Console.WriteLine($"Error : {ex.Message}");
            }
        
        }
         
        protected async Task DeleteWish(string wishlistId)
        {
            try{
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
                    await LoadMenus(currentPage, pageSize);
                 var url = $"/";
                Navigation.NavigateTo(url);

            }catch(Exception ex){
                Console.WriteLine($"Error : {ex.Message}");
            }
            
        }

    }

}