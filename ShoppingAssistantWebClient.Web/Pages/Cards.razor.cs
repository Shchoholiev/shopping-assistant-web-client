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

    //private static string[] Images = {
    //    "/images/return-card.png",
    //    "/images/exit.png",
    //    "/images/avatar.jpg"
    //};

    //public List<Product> Products = new()
    //{
    //    new Product {Id = "0", Url = "some link", Name = "Belkin USB C to VGA + Charge Adapter - USB C to VGA Cable for MacBook", 
    //                 Description = "The USB C to VGA + Charge Adapter connects to your laptop or tablet via USB-C port, giving you both a VGA port for video display and a USB-C port for power", Rating = 3.8, Price = 120, ImagesUrls = Images, WasOpened = false, WishlistId = "0"},
    //    new Product {Id = "1", Url = "some link", Name = "Second product", 
    //                 Description = "Test description", Rating = 4.2, Price = 30, ImagesUrls = Images, WasOpened = false, WishlistId = "0"}
    //};
    
    public List<Product> Products {get; set;}
    //public List<String> productsNames {get; set;}

    protected override async Task OnInitializedAsync()
    {   
        if (Products != null) {
            if(Products[currentProduct].ImagesUrls.Length > 0) {
                currentImage = Products[currentProduct].ImagesUrls[currentIndex];
            }
        }
        else {
            //productsNames = _searchService.Products;
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

    private async void LoadNextProduct() 
    {
        currentProduct += 1;
        StateHasChanged();
    }

    private async void LoadPreviousProduct() {
        currentProduct -= 1;
        StateHasChanged();
    }

    private void LoadMoreProducts() {

    }
}
