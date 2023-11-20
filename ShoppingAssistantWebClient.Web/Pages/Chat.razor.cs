using Microsoft.AspNetCore.Components;
using ShoppingAssistantWebClient.Web.Models;
using GraphQL;
using Newtonsoft.Json;
using ShoppingAssistantWebClient.Web.Network;
using ShoppingAssistantWebClient.Web.Models.Input;
using ShoppingAssistantWebClient.Web.Models.Enums;
using System.Text.RegularExpressions;
using Microsoft.JSInterop;

namespace ShoppingAssistantWebClient.Web.Pages;


public partial class Chat : ComponentBase
{

        [Inject]
        private ApiClient _apiClient { get; set; }
        [Inject]
        private NavigationManager Navigation { get; set; }

        public List<Messages> Messages { get; set; }
        public List<String> Suggestion { get; set; } = new List<String>();
        
        public Messages Message { get; set; }

        private CancellationTokenSource cancelTokenSource; 

        private MessageCreateDto messageCreateDto;
        private bool isWaitingForResponse = false;
        public bool isLoading = true;
        private string inputValue = "";
        private string name = "";
        protected override async Task OnInitializedAsync()
        {
            await LoadMessages();
        }


        private async Task LoadMessages()
        {
            try{
                string wishlistId = chatId;
        
                var request = new GraphQLRequest
                {
                    Query = @"query PersonalWishlist( $wishlistId: String!) {
                            personalWishlist(wishlistId: $wishlistId) {
                                name
                            }
                        }",

                    Variables = new
                    {
                        wishlistId,
                    }
                };

            var response = await _apiClient.QueryAsync(request);
            var responseData = response.Data;
            name = responseData.personalWishlist.name;


                isLoading = true;
                int pageNumber = 1;
                request = new GraphQLRequest
                {
                    Query = @"query MessagesPageFromPersonalWishlist($wishlistId: String!, $pageNumber: Int!, $pageSize: Int!) {
                                messagesPageFromPersonalWishlist( wishlistId: $wishlistId, pageNumber: $pageNumber, pageSize: $pageSize) 
                                {
                                    items {
                                        id
                                        text
                                        role
                                        createdById
                                    }
                                }
                            }",

                    Variables = new
                    {
                        wishlistId,
                        pageNumber,
                        pageSize = 200
                    }
                };
            
           
            
             response = await _apiClient.QueryAsync(request);
            responseData = response.Data;
                var jsonCategoriesResponse = JsonConvert.SerializeObject(responseData.messagesPageFromPersonalWishlist.items);
                this.Messages = JsonConvert.DeserializeObject<List<Messages>>(jsonCategoriesResponse);
                Messages.Reverse();
                isLoading = false;

            }catch(Exception ex){
                Console.WriteLine($"Error : {ex.Message}");
            }
        }
    private async Task AddNewMessage()
    {


        if (!isWaitingForResponse && !string.IsNullOrWhiteSpace(inputValue))
        {
            JSRuntime.InvokeVoidAsync("clearInput");
            isWaitingForResponse = true;

            try{
                
            messageCreateDto = new MessageCreateDto { Text = inputValue };;
            Message = new Messages();
            Message.Text = inputValue;
            Message.Role = "User";
            Message.Id = "";
            Message.CreatedById = "";
            inputValue = "";
            Suggestion = new List<String>();
            Messages.Add(Message);
            StateHasChanged();

            cancelTokenSource = new CancellationTokenSource();
            var cancellationToken = cancelTokenSource.Token;

            var serverSentEvent = _apiClient.GetServerSentEventStreamed($"ProductsSearch/search/{chatId}", messageCreateDto, cancellationToken);
            bool first = true;

            await foreach (var sseEvent in serverSentEvent.WithCancellation(cancellationToken))
            {
                Console.WriteLine($"Received SSE Event: {sseEvent.Event}, Data: {sseEvent.Data}");


                if(sseEvent.Event == SearchEventType.Message){

                    string input = sseEvent.Data;
                    Regex regex = new Regex("\"(.*?)\"");
                    Match match = regex.Match(input);
                    string result = match.Groups[1].Value;
        


                Message = new Messages();
                Message.Text = result;
                Message.Role = "bot";
                Message.Id = "";
                Message.CreatedById = "";

                if (first)
                {
                    Messages.Add(Message);
                    first = false;
                }
                else
                {
                    var lengt = Messages.Count();
                    Messages[lengt-1].Text += Message.Text;
                }

                StateHasChanged();
                    
                }else if(sseEvent.Event == SearchEventType.Product){

                    var url = $"/chat/{chatId}/product";
                    Navigation.NavigateTo(url);

                }else if(sseEvent.Event == SearchEventType.Suggestion){

                        Suggestion.Add(sseEvent.Data);
                }

            }

            isWaitingForResponse = false;
            }catch(Exception ex){
                    Console.WriteLine($"Error : {ex.Message}");
                }
        }

    }

}
