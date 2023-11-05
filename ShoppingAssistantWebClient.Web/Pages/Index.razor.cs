using Microsoft.AspNetCore.Components;
using ShoppingAssistantWebClient.Web.Models;
using GraphQL;
using Newtonsoft.Json;
using ShoppingAssistantWebClient.Web.Network;


namespace ShoppingAssistantWebClient.Web.Pages
{
    public partial class Index : ComponentBase
    {

         [Inject]
        private ApiClient _apiClient { get; set; }
         [Inject]
        private NavigationManager Navigation { get; set; }
        
        private string inputValue = "";
        public bool isLoading = true;


        private async Task CreateNewChat() {

            if(inputValue!=""){
                
            var type = selectedChoice;
    
            var firstMessageText= inputValue;
            var request = new GraphQLRequest
            {
                Query = @"mutation StartPersonalWishlist($type: String!, $firstMessageText: String!) {
                        startPersonalWishlist(dto: { type: $type, firstMessageText: $firstMessageText }) {
                            id
                        }
                    }
                    ",

                Variables = new
                {
                    type,
                    firstMessageText
                }
            };

            var response = await _apiClient.QueryAsync(request);
            var responseData = response.Data;
            var chat_id = responseData.startPersonalWishlist.id;
            var url = $"/chat/{chat_id}";
            Navigation.NavigateTo(url);

            }
                

        }

    }
}
