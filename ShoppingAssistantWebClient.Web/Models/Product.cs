namespace ShoppingAssistantWebClient.Web.Models;

public class Product 
{
    public required string Id {get; set;}

    public required string Url {get; set;}

    public required string Name {get; set;}

    public required string Description {get; set;}
    
    public required double Rating {get; set;}

    public required double Price { get; set; }

    public required string[] ImagesUrls {get; set;}

    public required bool WasOpened {get; set;}

    public required string WishlistId {get; set;}
}