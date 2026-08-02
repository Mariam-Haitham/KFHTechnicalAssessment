using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using TechnicalAssessment.BlazorUI.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// Register HttpClient for API calls.
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("http://localhost:5135")
});

// Register API clients.
builder.Services.AddScoped<CustomerApiClient>();
builder.Services.AddScoped<AccountApiClient>();

await builder.Build().RunAsync();
