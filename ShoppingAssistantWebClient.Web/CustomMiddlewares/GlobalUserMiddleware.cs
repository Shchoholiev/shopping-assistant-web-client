using ShoppingAssistantWebClient.Web.Models.GlobalInstances;
using ShoppingAssistantWebClient.Web.Network;
using System.Security.Authentication;

namespace ShoppingAssistantWebClient.Web.CustomMiddlewares;

public class GlobalUserMiddleware
{
    private readonly RequestDelegate _next;

    public GlobalUserMiddleware(RequestDelegate next)
    {
        this._next = next;
    }

public async Task InvokeAsync(HttpContext httpContext, AuthenticationService authenticationService, ApiClient apiClient)
    {
        if (httpContext.Request.Path != "/login")
        {
            try
            {
                var accessToken = await authenticationService.GetAuthTokenAsync();
                if (!string.IsNullOrEmpty(accessToken))
                {
                    apiClient.JwtToken = accessToken;
                    GlobalUser.Roles = authenticationService.GetRolesFromJwtToken(accessToken);
                    GlobalUser.Id = authenticationService.GetIdFromJwtToken(accessToken);
                    GlobalUser.Email = authenticationService.GetEmailFromJwtToken(accessToken);
                    GlobalUser.Phone = authenticationService.GetPhoneFromJwtToken(accessToken);
                }
            }
            catch (AuthenticationException ex)
            {
                httpContext.Response.Cookies.Delete("accessToken");
                httpContext.Response.Redirect("/login");
            }
        }
        await _next(httpContext);
    }
}