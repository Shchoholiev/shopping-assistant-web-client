using Microsoft.AspNetCore.Components;
using ShoppingAssistantWebClient.Web.Models;
using ShoppingAssistantWebClient.Web.Models.ProductSearch;
using ShoppingAssistantWebClient.Web.Models.Input;
using GraphQL;
using Newtonsoft.Json;
using ShoppingAssistantWebClient.Web.Network;
using System;
using Microsoft.JSInterop;
using ShoppingAssistantWebClient.Web.Services;

namespace ShoppingAssistantWebClient.Web.Pages
{
    public partial class Index : ComponentBase
    {

         [Inject]
        private ApiClient _apiClient { get; set; }
         [Inject]
         
        private NavigationManager Navigation { get; set; }
        [Inject]
        protected IJSRuntime JSRuntime { get; set; }
        [Inject]
        private SearchService _searchServise { get; set; }
        private MessageCreateDto messageCreateDto;
        private string inputValue = "";

        private async Task CreateNewChat() {

          try
            {
                if (string.IsNullOrWhiteSpace(inputValue))
                {
                    return;
                }

                StateHasChanged();
                messageCreateDto = new MessageCreateDto { Text = inputValue };
                var type = selectedChoice;
                var firstMessageText = $"What are you looking for?";

                var request = new GraphQLRequest
                {
                    Query = @"
                        mutation StartPersonalWishlist($type: String!, $firstMessageText: String!) {
                            startPersonalWishlist(dto: { type: $type, firstMessageText: $firstMessageText }) {
                                id
                            }
                        }",
                    Variables = new
                    {
                        type,
                        firstMessageText
                    }
                };

                var response = await _apiClient.QueryAsync(request);
                var responseData = response.Data;
                var chatId = responseData?.startPersonalWishlist?.id;
                string wishlistId = chatId;


                _searchServise.SetFirstMessage(inputValue);
                await UpdateSideMenu(wishlistId);

                var url = $"/chat/{chatId}";
                Navigation.NavigateTo(url);

            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately
                Console.WriteLine($"Error in CreateNewChat: {ex.Message}");
            }

        }

    }
}
