using Microsoft.AspNetCore.Components;
using ShoppingAssistantWebClient.Web.Network;
using ShoppingAssistantWebClient.Web.Models;
using ShoppingAssistantWebClient.Web.Models.GlobalInstances;
using GraphQL;
using Newtonsoft.Json;


namespace ShoppingAssistantWebClient.Web.Pages;

public partial class Settings : ComponentBase
{
    [Inject]
    private ApiClient _apiClient { get; set; }

    [Inject]
    private IHttpContextAccessor _httpContextAccessor { get; set; }

    public User user = new User();

    private string errorMessage = "";

    private string updateMessage = "";

    protected override async Task OnInitializedAsync()
    {
        await GetUser();
    }

    public async Task GetUser() {
        try {
            var request = new GraphQLRequest {
                Query = @"
                    query User($id: String!) { 
                        user(id: $id) { 
                            roles {
                                id
                                name
                            }
                            phone
                            email 
                        }
                    }",
                Variables = new
                {
                    id = GlobalUser.Id,
                }
            };

            var response = await _apiClient.QueryAsync(request);
            var responseData = response.Data;
            //System.Console.WriteLine(responseData);

            var jsonCategoriesResponse = JsonConvert.SerializeObject(responseData.user);
            this.user = JsonConvert.DeserializeObject<User>(jsonCategoriesResponse);
            user.GuestId = _httpContextAccessor.HttpContext.Request.Cookies["guestId"];

        }
        catch(Exception ex)
        {
            Console.WriteLine($"Error in GetUser: {ex}");
        }
    }

    public async Task UpdateUser()
    {
        try {
            if(user.Roles.Any(role => role.Name == "User"))
            {
                updateMessage = "Your data has been successfully updated";
            }
            if(phone == "") {
                phone = user.Phone;
            }
            if(email == "") {
                email = user.Email;
            }
            var request = new GraphQLRequest
            {
                Query = @"
                    mutation UpdateUser($userDto: UserDtoInput!) { 
                        updateUser(userDto: $userDto) { 
                            tokens { accessToken, refreshToken }, 
                            user { email, phone } 
                        }
                    }",
                Variables = new
                {
                    userDto = new
                    {
                        id = GlobalUser.Id,
                        guestId = user.GuestId,
                        roles = user.Roles.Select(r => new { id = r.Id, name = r.Name }),
                        email = email,
                        phone = phone,
                        password = password
                    }
                }
            };

            var response = await _apiClient.QueryAsync(request);
            var responseData = response.Data;
            System.Console.WriteLine(responseData);
            errorMessage = "";
            phone = "";
            email = "";
        } 
        catch(Exception ex)
        {
            if (ex.Message.Contains("The HTTP request failed with status code InternalServerError")) {
                errorMessage = "This user is already registered.";
            } else {
                errorMessage = "Something went wrong, please try again.";
            }
            Console.WriteLine($"Error in UpdateUser: {ex}");
        }
    }
}
