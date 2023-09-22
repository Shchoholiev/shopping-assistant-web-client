namespace ShoppingAssistantWebClient.Web.Models.GlobalInstances;

public static class GlobalUser
{
    public static string? Id { get; set; }

    public static string? Email { get; set; }

    public static string? Phone { get; set; }

    public static List<string>? Roles { get; set; } = new List<string>();
}
