using Microsoft.AspNetCore.Components;
using ShoppingAssistantWebClient.Web.Models;
using ShoppingAssistantWebClient.Web.Network;
using GraphQL;
using Newtonsoft.Json;
using Microsoft.JSInterop;
using ShoppingAssistantWebClient.Web.Services;

namespace ShoppingAssistantWebClient.Web.Pages;

public partial class Cards 
{
    [Inject]
    private ApiClient _apiClient { get; set; }

    [Inject]
    SearchService _searchService { get; set; }

    private int currentIndex = 0;

    private int currentProduct = 0;

    private string currentImage {get; set;}

    private bool isProductsNull = false;
    
    public List<Product> Products {get; set;} = new List<Product>();

    public List<String> ProductsNames {get; set;}

    private List<bool> isProductSaved { get; set; }

    protected override async Task OnInitializedAsync()
    {   
        ProductsNames = _searchService.Products;

        if (ProductsNames != null && ProductsNames.Any())
        {
        
            foreach (var productName in ProductsNames)
            {
                var newProduct = new Product
                {   
                    Id = "",
                    Url = "link",
                    Name = productName,
                    Description = "",
                    Rating = 0.0,
                    Price = 0.0,
                    ImagesUrls = new string[0],
                    WasOpened = false,
                    WishlistId = chatId
                };

                Products.Add(newProduct);
            }

            isProductSaved = new List<bool>(new bool[Products.Count]);
        }

        if (Products[currentProduct].ImagesUrls.Length > 0) 
        {
            currentImage = Products[currentProduct].ImagesUrls[currentIndex];
        }
        else {
            currentImage = "";
            isProductsNull = true;
        }
    }

    private void ShowNextImage(string image) 
    {
        currentIndex = Array.IndexOf(Products[currentProduct].ImagesUrls, image);
        currentIndex = (currentIndex + 1) % Products[currentProduct].ImagesUrls.Length;
        if(currentIndex >= 3 || currentIndex >= Products[currentProduct].ImagesUrls.Length) {
            currentIndex = 0;
        }
        currentImage = Products[currentProduct].ImagesUrls[currentIndex];
        StateHasChanged();
    }

    private void ShowPreviousImage(string image) 
    {
        currentIndex = Array.IndexOf(Products[currentProduct].ImagesUrls, image);
        currentIndex = (currentIndex - 1) % Products[currentProduct].ImagesUrls.Length;
        if(currentIndex < 0) {
            currentIndex = Products[currentProduct].ImagesUrls.Length > 2 ? 2 : Products[currentProduct].ImagesUrls.Length - 1;
        }
        currentImage = Products[currentProduct].ImagesUrls[currentIndex];
        StateHasChanged();
    }

    private void ChangeImageDot(int index) 
    {
        if (index >= 0 && index < Products[currentProduct].ImagesUrls.Length) {
            currentIndex = index;
            currentImage = Products[currentProduct].ImagesUrls[currentIndex];
            StateHasChanged();
        }
    }

    private async void LoadNextProduct(string type) 
    {
        if(type == "like" && isProductSaved[currentProduct] == false) {
            isProductSaved[currentProduct] = true;
            await AddProductToCart(Products[currentProduct]);
        }
        currentProduct += 1;
        StateHasChanged();
    }

    private async void LoadPreviousProduct() {
        currentProduct = currentProduct == 0 ? 0 : --currentProduct;
        StateHasChanged();
    }

    private async Task AddProductToCart(Product product) {
        try {
            var request = new GraphQLRequest {
                Query = @"mutation AddProductToPersonalWishlist($wishlistId: String!, $url: String!, $name: String!, $description: String!, $rating: Float!, $price: Float!, $imagesUrls: [String!]!, $wasOpened: Boolean!) {
                        addProductToPersonalWishlist(
                            wishlistId: $wishlistId
                            dto: {
                                url: $url
                                name: $name
                                description: $description
                                rating: $rating
                                price: $price
                                imagesUrls: $imagesUrls
                                wasOpened: $wasOpened
                            }
                        ) {
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
                    }",
                    Variables = new {
                        wishlistId = product.WishlistId,
                        url = product.Url,
                        name = product.Name,
                        description = product.Description,
                        rating = product.Rating,
                        price = product.Price,
                        imagesUrls = product.ImagesUrls,
                        wasOpened = product.WasOpened
                    }
                };

            Console.WriteLine("Sending GraphQL request: " + request);

            var response = await _apiClient.QueryAsync(request);
            var responseData = response.Data;

        }
        catch(Exception ex)
        {
            Console.WriteLine($"Error in AddProductToCart: {ex}");
        }
    }

    private void LoadMoreProducts() {

    }
}
