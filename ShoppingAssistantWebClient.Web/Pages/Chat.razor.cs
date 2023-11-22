using Microsoft.AspNetCore.Components;
using ShoppingAssistantWebClient.Web.Models;
using GraphQL;
using Newtonsoft.Json;
using ShoppingAssistantWebClient.Web.Network;
using ShoppingAssistantWebClient.Web.Models.Input;
using ShoppingAssistantWebClient.Web.Models.Enums;
using System.Text.RegularExpressions;
using ShoppingAssistantWebClient.Web.Services;
using Microsoft.JSInterop;

namespace ShoppingAssistantWebClient.Web.Pages;


public partial class Chat : ComponentBase
{

        [Inject]
        private ApiClient _apiClient { get; set; }
        [Inject]
        private NavigationManager Navigation { get; set; }
        [Inject]
        private SearchService _searchServise { get; set; }

        public List<Messages> Messages { get; set; }

        public List<String> Products { get; set; } = new List<string>();
        public List<String> Suggestion { get; set; } = new List<String>();
        
        public Messages Message { get; set; }

        private CancellationTokenSource cancelTokenSource; 

        private MessageCreateDto messageCreateDto;
        public bool isLoading = true;
        private string inputValue = "";
        private string name = "";
        protected override async Task OnInitializedAsync()
        {
            var input = _searchServise.firstMassage;
            
            if (input!=null){

            await LoadMessages();

            await  AddNewMessage(input);

                string wishlistId = chatId;
                var request = new GraphQLRequest
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

                var response = await _apiClient.QueryAsync(request);
                _searchServise.SetFirstMassage(null);
                isLoading = false;
                await UpdateSideMenu(wishlistId);
                StateHasChanged();

            }else{
                await LoadMessages();
            }

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
    private async Task AddNewMessage(string inputMessage)
    {


        try{
            
        messageCreateDto = new MessageCreateDto { Text = inputMessage };;
        Message = new Messages();
        Message.Text = inputMessage;
        Message.Role = "User";
        Message.Id = "";
        Message.CreatedById = "";
        inputValue = "";
        Suggestion = new List<String>();
        Products = new List<String>();
        Messages.Add(Message);
        StateHasChanged();

        cancelTokenSource = new CancellationTokenSource();
        var cancellationToken = cancelTokenSource.Token;
        const string animationChars = ".oOo";
        int animationIndex = 0;


        var serverSentEvent = _apiClient.GetServerSentEventStreamed($"ProductsSearch/search/{chatId}", messageCreateDto, cancellationToken);
        bool first = true;


                Message = new Messages();
                Message.Role = "bot";
                Message.Id = "";
                Message.CreatedById = "";
                Message.Text = "Waiting for response";
                Messages.Add(Message);



            StateHasChanged();
            await Task.Delay(500); // Adjust delay as needed
            animationIndex = (animationIndex + 1) % animationChars.Length;

        await foreach (var sseEvent in serverSentEvent.WithCancellation(cancellationToken))
        {

            Console.WriteLine($"Received SSE Event: {sseEvent.Event}, Data: {sseEvent.Data}");

            string input = sseEvent.Data;
            Regex regex = new Regex("\"(.*?)\"");
            Match match = regex.Match(input);
            string result = match.Groups[1].Value;
            if(sseEvent.Event == SearchEventType.Message){

                if (first)
                {
                    var lengt = Messages.Count();
                    Messages[lengt-1].Text = result;
                    first = false;
                }
                else
                {
                    var lengt = Messages.Count();
                    Messages[lengt-1].Text += result;
                }    

                StateHasChanged();
                
            } else if(sseEvent.Event == SearchEventType.Product){

                Products.Add(result);

            } else if(sseEvent.Event == SearchEventType.Suggestion){

                Suggestion.Add(sseEvent.Data);
            }

        }

    
            if(Products.Count!=0) {
                string n = name;
                _searchServise.SetProducts(Products);
                var url = $"/cards/{name}/{chatId}";
                Navigation.NavigateTo(url);
            }

        } catch(Exception ex){
                Console.WriteLine($"Error : {ex.Message}");
        }
    }

}
