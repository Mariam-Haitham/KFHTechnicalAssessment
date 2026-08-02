using TechnicalAssessment.BlazorUI.Client.Pages;
using TechnicalAssessment.BlazorUI.Client.Services;
using TechnicalAssessment.BlazorUI.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

// Register HttpClient for API calls (used by both server and WASM).
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5135") });

// Register API clients (needed for server-side render in InteractiveAuto).
builder.Services.AddScoped<CustomerApiClient>();
builder.Services.AddScoped<AccountApiClient>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(TechnicalAssessment.BlazorUI.Client._Imports).Assembly);

app.Run();
