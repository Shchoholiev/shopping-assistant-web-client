using Microsoft.AspNetCore.Components;
using ShoppingAssistantWebClient.Web.Models.ProductSearch;
using ShoppingAssistantWebClient.Web.Models.Input;
using GraphQL;
using Newtonsoft.Json;
using ShoppingAssistantWebClient.Web.Network;
using System;


namespace ShoppingAssistantWebClient.Web.Pages
{
    public partial class Index : ComponentBase
    {

         [Inject]
        private ApiClient _apiClient { get; set; }
         [Inject]
        private NavigationManager Navigation { get; set; }
        private MessageCreateDto messageCreateDto;

        private CancellationTokenSource cancelTokenSource; 
        
        private string inputValue = "";
        public bool isLoading = true;


        private async Task CreateNewChat() {

    try
            {
                if (string.IsNullOrWhiteSpace(inputValue))
                {
                    return;
                }

                isLoading = true;
                messageCreateDto = new MessageCreateDto { Text = inputValue };
                var type = selectedChoice;
                var firstMessageText = $"[Question] What are you looking for? [Suggestions] " + inputValue;

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
                isLoading = false;
                var url = $"/chat/{chatId}";

                cancelTokenSource = new CancellationTokenSource();
                var cancellationToken = cancelTokenSource.Token;

                var serverSentEvent =  _apiClient.GetServerSentEventStreamed($"ProductsSearch/search/{chatId}", messageCreateDto, cancellationToken);
                Navigation.NavigateTo(url);
                await foreach (var sseEvent in serverSentEvent.WithCancellation(cancellationToken))
                {
                    // Handle each ServerSentEvent as needed
                    Console.WriteLine($"Received SSE Event: {sseEvent.Event}, Data: {sseEvent.Data}");
                }

                }
                catch (Exception ex)
                {
                    // Handle exceptions appropriately
                    Console.WriteLine($"Error in CreateNewChat: {ex.Message}");
                }
                finally
                {
                    isLoading = false;
                    cancelTokenSource?.Dispose();
                }
        }

    }
}
