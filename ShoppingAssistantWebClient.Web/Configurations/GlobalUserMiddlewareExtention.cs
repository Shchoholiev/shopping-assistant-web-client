using ShoppingAssistantWebClient.Web.CustomMiddlewares;

namespace ShoppingAssistantWebClient.Web.Configurations;

public static class GlobalUserMiddlewareExtention
{
    public static IApplicationBuilder ConfigureGlobalUserMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<GlobalUserMiddleware>();
        return app;
    }
}
