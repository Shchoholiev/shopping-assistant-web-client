using Blazored.Modal;
using GraphQL.Client.Http;
using ShoppingAssistantWebClient.Web.Configurations;
using ShoppingAssistantWebClient.Web.Data;
using ShoppingAssistantWebClient.Web.Network;
using ShoppingAssistantWebClient.Web.Services;
using Blazored.Modal;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddServerSideBlazor().AddCircuitOptions(options => { options.DetailedErrors = true; });
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddApiClient(builder.Configuration);
builder.Services.AddSingleton<SearchService>();
builder.Services.AddBlazoredModal();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

// Login moved to ApiClient
// app.ConfigureGlobalUserMiddleware();

app.Run();
