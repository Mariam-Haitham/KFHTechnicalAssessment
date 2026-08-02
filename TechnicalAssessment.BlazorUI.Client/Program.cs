using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// Register HttpClient for API calls.
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7235") });

await builder.Build().RunAsync();
