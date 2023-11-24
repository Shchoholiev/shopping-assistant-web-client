using GraphQL;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Components;
using ShoppingAssistantWebClient.Web.Models;
using ShoppingAssistantWebClient.Web.Network;

namespace ShoppingAssistantWebClient.Web.Pages;

public partial class Cart : ComponentBase
{

    [Inject]
    private ApiClient _apiClient { get; set; }

    public List<Product> Products { get; set; } = new List<Product>();

    public bool isError = false;

    protected override async Task OnInitializedAsync()
    {
        await GetData();
    }

    private async Task GetData() 
    {
        try {
            var request = new GraphQLRequest {
                Query = @"query ProductsPageFromPersonalWishlist($wishlistId: String!, $pageNumber: Int!, $pageSize: Int!) {
                            productsPageFromPersonalWishlist(wishlistId: $wishlistId, pageNumber: $pageNumber, pageSize: $pageSize) {
                                items {
                                    id
                                    url
                                    name
                                    description
                                    rating
                                    price
                                    imagesUrls
                                    wasOpened
                                    wishlistId
                                }
                            }
                        }",

                Variables = new {
                    wishlistId = currentWishlistId,
                    pageNumber = 1,
                    pageSize = 10,
                }
            };

            var response = await _apiClient.QueryAsync(request);
            var responseData = response.Data;
            if (response.Errors != null && response.Errors.Any()) {
                isError = true;
            } 
            else 
            {
                var jsonCategoriesResponse = JsonConvert.SerializeObject(responseData.productsPageFromPersonalWishlist.items);
                this.Products = JsonConvert.DeserializeObject<List<Product>>(jsonCategoriesResponse);
            }
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex);
            isError = true;
        }
    }
}
