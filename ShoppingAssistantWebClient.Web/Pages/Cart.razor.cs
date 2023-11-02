using Microsoft.AspNetCore.Components;
using ShoppingAssistantWebClient.Web.Models;

namespace ShoppingAssistantWebClient.Web.Pages;

public partial class Cart : ComponentBase
{
    public List<Product> Products = new()
    {
        new Product {Id = "0", Url = "some link", Name = "HDMI", Description = "super mega hdmi cabel", Rating = 3.8, ImagesUrls = new string[] {"link"}, WasOpened = false, WishlistId = "0"},
        new Product {Id = "1", Url = "some link", Name = "HDMI", Description = "super mega hdmi cabel", Rating = 3.8, ImagesUrls = new string[] {"link"}, WasOpened = false, WishlistId = "1"},
        new Product {Id = "2", Url = "some link", Name = "HDMI", Description = "super mega hdmi cabel", Rating = 3.8, ImagesUrls = new string[] {"link"}, WasOpened = false, WishlistId = "2"},
        new Product {Id = "1", Url = "some link", Name = "HDMI", Description = "super mega hdmi cabel", Rating = 3.8, ImagesUrls = new string[] {"link"}, WasOpened = false, WishlistId = "1"},
        new Product {Id = "1", Url = "some link", Name = "HDMI", Description = "super mega hdmi cabel", Rating = 3.8, ImagesUrls = new string[] {"link"}, WasOpened = false, WishlistId = "1"},
        new Product {Id = "1", Url = "some link", Name = "HDMI", Description = "super mega hdmi cabel", Rating = 3.8, ImagesUrls = new string[] {"link"}, WasOpened = false, WishlistId = "1"},
        new Product {Id = "1", Url = "some link", Name = "HDMI", Description = "super mega hdmi cabel", Rating = 3.8, ImagesUrls = new string[] {"link"}, WasOpened = false, WishlistId = "1"},
        new Product {Id = "1", Url = "some link", Name = "HDMI", Description = "super mega hdmi cabel", Rating = 3.8, ImagesUrls = new string[] {"link"}, WasOpened = false, WishlistId = "1"},
        new Product {Id = "1", Url = "some link", Name = "HDMI", Description = "super mega hdmi cabel", Rating = 3.8, ImagesUrls = new string[] {"link"}, WasOpened = false, WishlistId = "1"},
        new Product {Id = "1", Url = "some link", Name = "HDMI", Description = "super mega hdmi cabel", Rating = 3.8, ImagesUrls = new string[] {"link"}, WasOpened = false, WishlistId = "1"},
        new Product {Id = "1", Url = "some link", Name = "HDMI", Description = "super mega hdmi cabel", Rating = 3.8, ImagesUrls = new string[] {"link"}, WasOpened = false, WishlistId = "1"},
        new Product {Id = "1", Url = "some link", Name = "HDMI", Description = "super mega hdmi cabel", Rating = 3.8, ImagesUrls = new string[] {"link"}, WasOpened = false, WishlistId = "1"},
        new Product {Id = "1", Url = "some link", Name = "HDMI", Description = "super mega hdmi cabel", Rating = 3.8, ImagesUrls = new string[] {"link"}, WasOpened = false, WishlistId = "1"},
        new Product {Id = "1", Url = "some link", Name = "HDMI", Description = "super mega hdmi cabel", Rating = 3.8, ImagesUrls = new string[] {"link"}, WasOpened = false, WishlistId = "1"},
        new Product {Id = "1", Url = "some link", Name = "HDMI", Description = "super mega hdmi cabel", Rating = 3.8, ImagesUrls = new string[] {"link"}, WasOpened = false, WishlistId = "1"},
        new Product {Id = "1", Url = "some link", Name = "HDMI", Description = "super mega hdmi cabel", Rating = 3.8, ImagesUrls = new string[] {"link"}, WasOpened = false, WishlistId = "1"},
        new Product {Id = "1", Url = "some link", Name = "HDMI", Description = "super mega hdmi cabel", Rating = 3.8, ImagesUrls = new string[] {"link"}, WasOpened = false, WishlistId = "1"},
        new Product {Id = "1", Url = "some link", Name = "HDMI", Description = "super mega hdmi cabel", Rating = 3.8, ImagesUrls = new string[] {"link"}, WasOpened = false, WishlistId = "1"},
        new Product {Id = "1", Url = "some link", Name = "HDMI", Description = "super mega hdmi cabel", Rating = 3.8, ImagesUrls = new string[] {"link"}, WasOpened = false, WishlistId = "1"},
        new Product {Id = "1", Url = "some link", Name = "HDMI", Description = "super mega hdmi cabel", Rating = 3.8, ImagesUrls = new string[] {"link"}, WasOpened = false, WishlistId = "1"},
        new Product {Id = "1", Url = "some link", Name = "HDMI", Description = "super mega hdmi cabel", Rating = 3.8, ImagesUrls = new string[] {"link"}, WasOpened = false, WishlistId = "1"},
        new Product {Id = "1", Url = "some link", Name = "HDMI", Description = "super mega hdmi cabel", Rating = 3.8, ImagesUrls = new string[] {"link"}, WasOpened = false, WishlistId = "1"},
        new Product {Id = "1", Url = "some link", Name = "HDMI", Description = "super mega hdmi cabel", Rating = 3.8, ImagesUrls = new string[] {"link"}, WasOpened = false, WishlistId = "1"},
        new Product {Id = "1", Url = "some link", Name = "HDMI", Description = "super mega hdmi cabel", Rating = 3.8, ImagesUrls = new string[] {"link"}, WasOpened = false, WishlistId = "1"},
        new Product {Id = "1", Url = "some link", Name = "HDMI", Description = "super mega hdmi cabel", Rating = 3.8, ImagesUrls = new string[] {"link"}, WasOpened = false, WishlistId = "1"},
        new Product {Id = "1", Url = "some link", Name = "HDMI", Description = "super mega hdmi cabel", Rating = 3.8, ImagesUrls = new string[] {"link"}, WasOpened = false, WishlistId = "1"},
        new Product {Id = "1", Url = "some link", Name = "HDMI", Description = "super mega hdmi cabel", Rating = 3.8, ImagesUrls = new string[] {"link"}, WasOpened = false, WishlistId = "1"},
        new Product {Id = "1", Url = "some link", Name = "HDMI", Description = "super mega hdmi cabel", Rating = 3.8, ImagesUrls = new string[] {"link"}, WasOpened = false, WishlistId = "1"},
        new Product {Id = "1", Url = "some link", Name = "HDMI", Description = "super mega hdmi cabel", Rating = 3.8, ImagesUrls = new string[] {"link"}, WasOpened = false, WishlistId = "1"},
        new Product {Id = "1", Url = "some link", Name = "HDMI", Description = "super mega hdmi cabel", Rating = 3.8, ImagesUrls = new string[] {"link"}, WasOpened = false, WishlistId = "1"},
        new Product {Id = "1", Url = "some link", Name = "HDMI", Description = "super mega hdmi cabel", Rating = 3.8, ImagesUrls = new string[] {"link"}, WasOpened = false, WishlistId = "1"},
        new Product {Id = "1", Url = "some link", Name = "HDMI", Description = "super mega hdmi cabel", Rating = 3.8, ImagesUrls = new string[] {"link"}, WasOpened = false, WishlistId = "1"},
        new Product {Id = "1", Url = "some link", Name = "HDMI", Description = "super mega hdmi cabel", Rating = 3.8, ImagesUrls = new string[] {"link"}, WasOpened = false, WishlistId = "1"},
        new Product {Id = "1", Url = "some link", Name = "HDMI", Description = "super mega hdmi cabel", Rating = 3.8, ImagesUrls = new string[] {"link"}, WasOpened = false, WishlistId = "1"},
        new Product {Id = "1", Url = "some link", Name = "HDMI", Description = "super mega hdmi cabel", Rating = 3.8, ImagesUrls = new string[] {"link"}, WasOpened = false, WishlistId = "1"},
    };

    protected override async Task OnInitializedAsync()
    {
        // Get data from Back-end
    }
}