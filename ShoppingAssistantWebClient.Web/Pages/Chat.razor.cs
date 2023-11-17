using Microsoft.AspNetCore.Components;
using ShoppingAssistantWebClient.Web.Models;
using GraphQL;
using Newtonsoft.Json;
using ShoppingAssistantWebClient.Web.Network;
using ShoppingAssistantWebClient.Web.Models.Input;
using ShoppingAssistantWebClient.Web.Models.Enums;
using System.Text.RegularExpressions;
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
        public bool isLoading = true;
        private string inputValue = "";
        private string name = "";
        protected override async Task OnInitializedAsync()
        {
            await LoadMessages();
        }


        private async Task LoadMessages()
        {
            string wishlistId = chatId;
            
                                 Suggestion.Add("123");
                     Suggestion.Add("456");
                     Suggestion.Add("145623");

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
            try{
             response = await _apiClient.QueryAsync(request);
            responseData = response.Data;
                var jsonCategoriesResponse = JsonConvert.SerializeObject(responseData.messagesPageFromPersonalWishlist.items);
                this.Messages = JsonConvert.DeserializeObject<List<Messages>>(jsonCategoriesResponse);
                Messages.Reverse();
                isLoading = false;

            }catch{

            }

           /*
            try{


                    pageNumber = 0;

            do{
                pageNumber++;

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
                                                totalPages
                                            }
                                        }",

                                Variables = new
                                {
                                    wishlistId,
                                    pageNumber,
                                    pageSize = 1
                                }
                            };

                response = await _apiClient.QueryAsync(request);
                responseData = response.Data;
                var jsonCategoriesResponse = JsonConvert.SerializeObject(responseData.messagesPageFromPersonalWishlist.items);
                if(pageNumber == 1){
                    Messages =  JsonConvert.DeserializeObject<List<Messages>>(jsonCategoriesResponse);
                }else{
                    Message =  JsonConvert.DeserializeObject<List<Messages>>(jsonCategoriesResponse);
                    Messages.InsertRange(0, Message);
                }
              

              //  Messages.Reverse();
                totalPages = responseData.messagesPageFromPersonalWishlist.totalPages;
                isLoading = false;
                StateHasChanged();
            }while(pageNumber<totalPages);
             
            }catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
          */

        }
    private async Task AddNewMessage()
    {
        messageCreateDto = new MessageCreateDto { Text = inputValue };;
        Message = new Messages();
        Message.Text = inputValue;
        Message.Role = "User";
        Message.Id = "";
        Message.CreatedById = "";
        inputValue = "";
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

            await InvokeAsync(StateHasChanged);
                
            }else if(sseEvent.Event == SearchEventType.Product){

                var url = $"/chat/{chatId}/product";
                Navigation.NavigateTo(url);

            }else if(sseEvent.Event == SearchEventType.Suggestion){

                    Suggestion.Add(sseEvent.Data);
            }

        }

    }

}
