using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using ShoppingAssistantWebClient.Web.Network;

namespace ShoppingAssistantWebClient.Web.Configurations;

public static class DependencyInjectionExtension
{
    public static IServiceCollection AddApiClient(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor();

        var apiUrl = configuration.GetValue<string>("ApiUrl");
        services.AddHttpClient("ApiHttpClient", client => {
            client.BaseAddress = new Uri(apiUrl + "api/");
        });

        services.AddScoped<GraphQLHttpClient>(p =>
            new GraphQLHttpClient(apiUrl + "graphql", new NewtonsoftJsonSerializer())
        );

        services.AddScoped<AuthenticationService>();
        services.AddScoped<ApiClient>();

        return services;
    }
}