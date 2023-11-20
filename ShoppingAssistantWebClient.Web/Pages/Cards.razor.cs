using Microsoft.AspNetCore.Components;
using ShoppingAssistantWebClient.Web.Models;
using ShoppingAssistantWebClient.Web.Network;
using GraphQL;
using Newtonsoft.Json;
using Microsoft.JSInterop;

namespace ShoppingAssistantWebClient.Web.Pages;

public partial class Cards 
{
    [Inject]
    private ApiClient _apiClient { get; set; }

    private int currentIndex = 0;

    private int currentProduct = 0;

    private string currentImage {get; set;}

    private bool isProductsNull = false;

    //private static string[] Images = {
    //    "/images/image2.png",
    //    "/images/image1.png",
    //    "/images/return-card.png",
    //    "/images/amazon.png",
    //    "/images/avatar.png"
    //};

    //public List<Product> Products = new()
    //{
    //    new Product {Id = "0", Url = "some link", Name = "Belkin USB C to VGA + Charge Adapter - USB C to VGA Cable for MacBook", 
    //                 Description = "The USB C to VGA + Charge Adapter connects to your laptop or tablet via USB-C port, giving you both a VGA port for video display and a USB-C port for power", Rating = 3.8, Price = 120, ImagesUrls = Images, WasOpened = false, WishlistId = "0"},
    //    new Product {Id = "1", Url = "some link", Name = "Second product", 
    //                 Description = "Test description", Rating = 4.2, Price = 30, ImagesUrls = Images, WasOpened = false, WishlistId = "0"}
    //};
    
    public List<Product> Products {get; set;}

    protected override async Task OnInitializedAsync()
    {   
        if (Products != null) {
            currentImage = Products[currentProduct].ImagesUrls[currentIndex];
        }
        else {
            currentImage = "";
            isProductsNull = true;
        } 
    }

    private void ChangeImage(string image) 
    {
        currentIndex = Array.IndexOf(Products[currentProduct].ImagesUrls, image);
        currentIndex = (currentIndex + 1) % Products[currentProduct].ImagesUrls.Length;
        currentIndex = currentIndex >= 3 ? 0 : currentIndex;
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
