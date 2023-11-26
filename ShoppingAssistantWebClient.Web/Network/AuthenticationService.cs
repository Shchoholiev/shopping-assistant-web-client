using GraphQL;
using GraphQL.Client.Http;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ShoppingAssistantWebClient.Web.Models.Identity;
using ShoppingAssistantWebClient.Web.Models.Input;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Authentication;
using System.Security.Claims;

namespace ShoppingAssistantWebClient.Web.Network;

public class AuthenticationService
{
    private readonly HttpContext _httpContext;

    private readonly GraphQLHttpClient _graphQLClient;

    public AuthenticationService(
        IHttpContextAccessor httpContextAccessor,
        GraphQLHttpClient graphQLClient)
    {
        _httpContext = httpContextAccessor.HttpContext;
        _graphQLClient = graphQLClient;
    }

    public async Task<string> GetAuthTokenAsync()
    {
        var jwtToken = _httpContext.Request.Cookies["accessToken"];
        var refreshToken = _httpContext.Request.Cookies["refreshToken"];

        if (string.IsNullOrEmpty(jwtToken) && !string.IsNullOrEmpty(refreshToken))
        {
            throw new AuthenticationException("User is not Authenticated");
        }
        else if (string.IsNullOrEmpty(jwtToken) && string.IsNullOrEmpty(refreshToken))
        {
            jwtToken = await AccessGuest();
        }
        else if (IsJwtTokenExpired(jwtToken))
        {
            try
            {
                jwtToken = await RefreshToken();
            }
            catch (Exception ex)
            {
                throw new AuthenticationException("User is not Authenticated");
            }
        }

        return jwtToken;
    }

    public async Task LoginAsync(LoginInputModel model)
    {
        var request = new GraphQLRequest
        {
            Query = @"
                    mutation Login($login: AccessUserModelInput!) {
                        login(login: $login) {
                            accessToken
                            refreshToken
                        }
                    }
            ",
            Variables = new { login = new { email = model.Email, phone = model.Phone, password = model.Password } }
        };
        var response = await _graphQLClient.SendMutationAsync<dynamic>(request);
        var jsonResponse = JsonConvert.SerializeObject(response.Data.login);
        var tokens = JsonConvert.DeserializeObject<TokensModel>(jsonResponse);
        _httpContext.Response.Cookies.Append("accessToken", tokens.AccessToken, new CookieOptions { Expires = DateTime.UtcNow.AddDays(180) });
        _httpContext.Response.Cookies.Append("refreshToken", tokens.RefreshToken, new CookieOptions { Expires = DateTime.UtcNow.AddDays(180) });
    }

    public void SetTokens(TokensModel tokens)
    {
        _httpContext.Response.Cookies.Append("accessToken", tokens.AccessToken, new CookieOptions { Expires = DateTime.UtcNow.AddDays(180) });
        _httpContext.Response.Cookies.Append("refreshToken", tokens.RefreshToken, new CookieOptions { Expires = DateTime.UtcNow.AddDays(180) });
    }

    public bool IsJwtTokenExpired(string jwtToken)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var token = tokenHandler.ReadJwtToken(jwtToken);
        var expiration = token.ValidTo;
        var isExpired = expiration < DateTime.UtcNow;

        return isExpired;
    }

    public List<string> GetRolesFromJwtToken(string jwtToken)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.ReadJwtToken(jwtToken);
        var claims = token.Claims;
        var roleClaims = claims.Where(c => c.Type == ClaimTypes.Role);
        var roles = roleClaims.Select(c => c.Value).ToList();

        return roles;
    }

    public string? GetEmailFromJwtToken(string jwtToken)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.ReadJwtToken(jwtToken);
        var claims = token.Claims;
        var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

        return email;
    }

    public string? GetPhoneFromJwtToken(string jwtToken)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.ReadJwtToken(jwtToken);
        var claims = token.Claims;
        var phone = claims.FirstOrDefault(c => c.Type == ClaimTypes.MobilePhone)?.Value;

        return phone;
    }

    public string? GetIdFromJwtToken(string jwtToken)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.ReadJwtToken(jwtToken);
        var claims = token.Claims;
        var id = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        return id;
    }

    private async Task<string> AccessGuest()
    {
        var guestId = _httpContext.Request.Cookies["guestId"];
        if (string.IsNullOrEmpty(guestId))
        {
            guestId = Guid.NewGuid().ToString();
            _httpContext.Response.Cookies.Append("guestId", guestId, new CookieOptions { Expires = DateTime.UtcNow.AddDays(180) });
        }

        var request = new GraphQLRequest
        {
            Query = @"
                    mutation AccessGuest($guest: AccessGuestModelInput!) {
                        accessGuest(guest: $guest) {
                            accessToken
                            refreshToken
                        }
                    }",
            Variables = new { guest = new { guestId = guestId } }
        };

        var response = await _graphQLClient.SendMutationAsync<dynamic>(request);
        var jsonResponse = JsonConvert.SerializeObject(response.Data.accessGuest);
        var tokens = JsonConvert.DeserializeObject<TokensModel>(jsonResponse);
        _httpContext.Response.Cookies.Append("accessToken", tokens.AccessToken, new CookieOptions { Expires = DateTime.UtcNow.AddDays(180) });
        _httpContext.Response.Cookies.Append("refreshToken", tokens.RefreshToken, new CookieOptions { Expires = DateTime.UtcNow.AddDays(180) });

        return tokens.AccessToken;
    }

    private async Task<string> RefreshToken()
    {
        var accessToken = _httpContext.Request.Cookies["accessToken"];
        var refreshToken = _httpContext.Request.Cookies["refreshToken"];

        var request = new GraphQLRequest
        {
            Query = @"
                    mutation RefreshAccessToken($model: TokensModelInput!) {
                        refreshAccessToken(model: $model) {
                            accessToken
                            refreshToken
                        }
                    }",
            Variables = new { model = new { accessToken = accessToken, refreshToken = refreshToken } }
        };
        var response = await _graphQLClient.SendMutationAsync<dynamic>(request);
        var jsonResponse = JsonConvert.SerializeObject(response.Data.refreshAccessToken);
        var tokens = JsonConvert.DeserializeObject<TokensModel>(jsonResponse);
        _httpContext.Response.Cookies.Append("accessToken", tokens.AccessToken, new CookieOptions { Expires = DateTime.UtcNow.AddDays(180) });
        _httpContext.Response.Cookies.Append("refreshToken", tokens.RefreshToken, new CookieOptions { Expires = DateTime.UtcNow.AddDays(180) });

        return tokens.AccessToken;
    }
}
