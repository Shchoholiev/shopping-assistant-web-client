using Microsoft.AspNetCore.Components;
using ShoppingAssistantWebClient.Web.Models;
using ShoppingAssistantWebClient.Web.Models.ProductSearch;
using ShoppingAssistantWebClient.Web.Models.Input;
using GraphQL;
using Newtonsoft.Json;
using ShoppingAssistantWebClient.Web.Network;
using System;
using Microsoft.JSInterop;

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

        private MessageCreateDto messageCreateDto;

        private CancellationTokenSource cancelTokenSource; 
        
        private string inputValue = "";
        public bool isLoading;


        private async Task CreateNewChat() {

          try
            {
                if (string.IsNullOrWhiteSpace(inputValue))
                {
                    return;
                }

                isLoading = true;
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
                                string wishlistId1 = chatId;

                var text = inputValue;
/*
                inputValue="";
                request = new GraphQLRequest
                {
                    Query = @"mutation AddMessageToPersonalWishlist($wishlistId: String!, $text: String!) {
                                addMessageToPersonalWishlist(wishlistId: $wishlistId, dto: { text: $text }) {
                                    id
                                    text
                                    role
                                    createdById
                                }
                            }
                            ",

                    Variables = new
                    {
                        wishlistId =chatId,
                        text
                    }
                };

                 await _apiClient.QueryAsync(request);
*/


                cancelTokenSource = new CancellationTokenSource();
                var cancellationToken = cancelTokenSource.Token;

                var serverSentEvent =  _apiClient.GetServerSentEventStreamed($"ProductsSearch/search/{chatId}", messageCreateDto, cancellationToken);

                await foreach (var sseEvent in serverSentEvent.WithCancellation(cancellationToken))
                {
                    // Handle each ServerSentEvent as needed
                    Console.WriteLine($"Received SSE Event: {sseEvent.Event}, Data: {sseEvent.Data}");
                }

                string wishlistId = chatId;

                request = new GraphQLRequest
                {
                    Query = @"mutation GenerateNameForPersonalWishlist($wishlistId: String!) {
                            generateNameForPersonalWishlist(wishlistId: $wishlistId) {
                                id
                                name
                            }
                        }",
                     Variables = new
                    {
                        wishlistId
                        
                    }
                };

                response = await _apiClient.QueryAsync(request);

                isLoading = false;
                StateHasChanged();

                await UpdateSideMenu(wishlistId1);
                var url = $"/chat/{chatId}";
                Navigation.NavigateTo(url);

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
