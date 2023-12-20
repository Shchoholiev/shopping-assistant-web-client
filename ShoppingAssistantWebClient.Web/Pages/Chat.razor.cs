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
    public Messages MessageBot { get; set; }

    private CancellationTokenSource cancelTokenSource;
    private bool isWaitingForResponse = false;
    private MessageCreateDto messageCreateDto;
    public bool isLoading = true;
    private string name = "";
    protected override async Task OnInitializedAsync()
    {
        try
        {
            var input = _searchServise.FirstMessage;

            if (input != null)
            {

                await LoadMessages();

                await AddNewMessage(input);

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
                _searchServise.SetFirstMessage(null);
                isLoading = false;
                await UpdateSideMenu(wishlistId);
                StateHasChanged();

            }
            else
            {
                await LoadMessages();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error OnInitializedAsync: {ex.Message}");
        }

    }


    private async Task LoadMessages()
    {
        try
        {
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

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error : {ex.Message}");
        }
    }
    private async Task AddNewMessage(string inputMessage)
    {

        if (!isWaitingForResponse && !string.IsNullOrWhiteSpace(inputMessage))
        {
            JSRuntime.InvokeVoidAsync("clearInput");
            isWaitingForResponse = true;

            try
            {
                messageCreateDto = new MessageCreateDto { Text = inputMessage }; ;
                Message = new Messages();
                Message.Text = inputMessage;
                Message.Role = "User";
                Message.Id = "";
                Message.CreatedById = "";

                Suggestion = new List<String>();
                Products = new List<String>();
                Messages.Add(Message);
                StateHasChanged();

                cancelTokenSource = new CancellationTokenSource();
                var cancellationToken = cancelTokenSource.Token;

                var serverSentEvent = _apiClient.GetServerSentEventStreamed($"ProductsSearch/search/{chatId}", messageCreateDto, cancellationToken);
                bool first = true;

                MessageBot = new Messages();
                MessageBot.Role = "bot";
                MessageBot.Id = "";
                MessageBot.CreatedById = "";
                MessageBot.Text = "Waiting for response";
                Messages.Add(MessageBot);
                var lengt = Messages.Count();
                StateHasChanged();

                await foreach (var sseEvent in serverSentEvent.WithCancellation(cancellationToken))
                {
                    Console.WriteLine($"Received SSE Event: {sseEvent.Event}, Data: {sseEvent.Data}");

                    string input = sseEvent.Data;
                    Regex regex = new Regex("\"(.*?)\"");
                    Match match = regex.Match(input);
                    string result = match.Groups[1].Value;

                    if (sseEvent.Event == SearchEventType.Message)
                    {
                        if (first)
                        {
                            Messages[lengt - 1].Text = result;
                            first = false;
                        }
                        else
                        {
                            Messages[lengt - 1].Text += result;
                        }

                        StateHasChanged();

                    }
                    else if (sseEvent.Event == SearchEventType.Product)
                    {

                        string pattern = "[\\\\\"]";

                        input = Regex.Replace(input, pattern, "");

                        Products.Add(input);

                    }
                    else if (sseEvent.Event == SearchEventType.Suggestion)
                    {
                        if (Suggestion.Count < 3)
                        {
                            Suggestion.Add(result);
                            StateHasChanged();
                        }
                    }
                }

                if (Products.Count != 0)
                {
                    string n = name;
                    _searchServise.SetProducts(Products);
                    Products = null;
                    var url = $"/cards/{name}/{chatId}";
                    Navigation.NavigateTo(url);
                }
                isWaitingForResponse = false;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error : {ex.Message}");
            }
        }
    }
}
