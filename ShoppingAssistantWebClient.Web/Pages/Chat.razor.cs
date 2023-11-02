using Microsoft.AspNetCore.Components;
using ShoppingAssistantWebClient.Web.Models;
using GraphQL;
using Newtonsoft.Json;
using ShoppingAssistantWebClient.Web.Network;

namespace ShoppingAssistantWebClient.Web.Pages;

public partial class Chat : ComponentBase
{

    [Inject]
    private ApiClient _apiClient { get; set; }


    protected override async Task OnInitializedAsync()
        {
            await LoadMenus();
        }


        private async Task LoadMenus()
        {
            var pageNumber = 1;
            var request = new GraphQLRequest
            {
                Query = @"mutation StartPersonalWishlist {
                        startPersonalWishlist(dto: { type: product, firstMessageText: hello }) {
                            id
                            name
                            type
                            createdById
                        }
                    }",

                Variables = new
                {
                    pageNumber = pageNumber,
                    pageSize = 12,
                }
            };

        var response = await _apiClient.QueryAsync(request);

    }




}
