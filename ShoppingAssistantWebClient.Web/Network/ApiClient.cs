using GraphQL.Client.Http;
using GraphQL;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using ShoppingAssistantWebClient.Web.Models.GlobalInstances;

namespace ShoppingAssistantWebClient.Web.Network;

public class ApiClient
{
    private readonly HttpClient _httpClient;

    private readonly GraphQLHttpClient _graphQLClient;

    private readonly AuthenticationService _authenticationService;

    public string JwtToken;

    public ApiClient(
        IHttpClientFactory httpClientFactory,
        GraphQLHttpClient graphQLClient,
        AuthenticationService authenticationService)
    {
        _httpClient = httpClientFactory.CreateClient("ApiHttpClient");
        _graphQLClient = graphQLClient;
        _authenticationService = authenticationService;
    }

    public async Task<dynamic> QueryAsync(GraphQLRequest request)
    {
        await SetAuthenticationAsync();

        var response = await _graphQLClient.SendQueryAsync<dynamic>(request);

        return response;
    }

    public async Task<T> QueryAsync<T>(GraphQLRequest request, string propertyName)
    {
        await SetAuthenticationAsync();

        var response = await _graphQLClient.SendQueryAsync<dynamic>(request);
        var obj = response.Data.GetValue(propertyName);
        var jsonResponse = JsonConvert.SerializeObject(obj);

        var model = JsonConvert.DeserializeObject<T>(jsonResponse);
        return model;
    }

    public async Task<T> PostFormAsync<T>(string url, IFormCollection formCollection)
    {
        await SetAuthenticationAsync();

        var formData = MapIFormCollectionToForm(formCollection);
        var response = await _httpClient.PostAsync(url, formData);
        var responseContent = await response.Content.ReadAsStringAsync();

        var model = JsonConvert.DeserializeObject<T>(responseContent);
        return model;
    }

    public async Task<T> PostJsonAsync<T>(string url, Object obj)
    {
        await SetAuthenticationAsync();

        var response = await _httpClient.PostAsJsonAsync(url, obj);
        var responseContent = await response.Content.ReadAsStringAsync();

        var model = JsonConvert.DeserializeObject<T>(responseContent);
        return model;
    }

    public async Task<T> PutFormAsync<T>(string url, IFormCollection formCollection)
    {
        await SetAuthenticationAsync();

        var formData = MapIFormCollectionToForm(formCollection);
        var response = await _httpClient.PutAsync(url, formData);
        var responseContent = await response.Content.ReadAsStringAsync();

        var model = JsonConvert.DeserializeObject<T>(responseContent);
        return model;
    }

    public async Task<T> PutJsonAsync<T>(string url, Object obj)
    {
        await SetAuthenticationAsync();

        var response = await _httpClient.PutAsJsonAsync(url, obj);
        var responseContent = await response.Content.ReadAsStringAsync();

        var model = JsonConvert.DeserializeObject<T>(responseContent);
        return model;
    }

    private MultipartFormDataContent MapIFormCollectionToForm(IFormCollection formCollection)
    {
        var formDataContent = new MultipartFormDataContent();

        foreach (var key in formCollection.Keys)
        {
            foreach (var value in formCollection[key])
            {
                if (value != null)
                {
                    formDataContent.Add(new StringContent(value), key);
                }
            }
        }

        foreach (var file in formCollection.Files)
        {
            var fileContent = new StreamContent(file.OpenReadStream());
            formDataContent.Add(fileContent, file.Name, file.FileName);
        }

        return formDataContent;
    }

    private async Task SetAuthenticationAsync()
    {
        var accessToken = await _authenticationService.GetAuthTokenAsync();
        if (!string.IsNullOrEmpty(accessToken))
        {
            this.JwtToken = accessToken;

            GlobalUser.Id = _authenticationService.GetIdFromJwtToken(accessToken);
            GlobalUser.Email = _authenticationService.GetEmailFromJwtToken(accessToken);
            GlobalUser.Phone = _authenticationService.GetPhoneFromJwtToken(accessToken);
            GlobalUser.Roles = _authenticationService.GetRolesFromJwtToken(accessToken);

            _graphQLClient.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.JwtToken);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.JwtToken);
        }
    }
}