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

        public List<Messages> Messages { get; set; }
        public bool isLoading = true;
        private string inputValue = "";
        protected override async Task OnInitializedAsync()
        {
            await LoadMessages();
        }


        private async Task LoadMessages()
        {


            isLoading = true;
            int pageNumber = 1;
            string wishlistId = chatId;
            var request = new GraphQLRequest
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
                    pageSize = 20
                }
            };
            try{
            var response = await _apiClient.QueryAsync(request);
                var responseData = response.Data;
                var jsonCategoriesResponse = JsonConvert.SerializeObject(responseData.messagesPageFromPersonalWishlist.items);
                this.Messages = JsonConvert.DeserializeObject<List<Messages>>(jsonCategoriesResponse);
                Messages.Reverse();
                isLoading = false;

            }catch{

            }
            

        }
        private async Task AddNewMessage()
        {

            isLoading = true;
            var pageNumber = 1;
            var wishlistId = chatId;
            var text = inputValue;
            inputValue="";
            var request = new GraphQLRequest
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
                    wishlistId,
                    text
                }
            };

            var response = await _apiClient.QueryAsync(request);
            await LoadMessages();
        }





}
