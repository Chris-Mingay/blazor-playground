using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Blazor.App;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddAuthorizationCore();
//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddSingleton<JwtAuthenticationStateProvider>();
builder.Services.AddSingleton<AuthenticationStateProvider>(provider => provider.GetRequiredService<JwtAuthenticationStateProvider>());
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddTransient<CookieHandler>();

var webApiUri = new Uri("https://localhost:7062/api");
builder.Services.AddScoped(provider => new JwtTokenMessageHandler(webApiUri, provider.GetRequiredService<JwtAuthenticationStateProvider>()));

builder.Services.AddHttpClient("Web.Api", client => client.BaseAddress = webApiUri)
    .AddHttpMessageHandler<JwtTokenMessageHandler>()
    .AddHttpMessageHandler<CookieHandler>();
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("Web.Api"));

builder.Services.AddHttpClient<IAuthenticationService, AuthenticationService>("Web.Api.Auth", client =>
{
    client.BaseAddress = new Uri("https://localhost:7062/api/auth/");
})
    .AddHttpMessageHandler<JwtTokenMessageHandler>()
    .AddHttpMessageHandler<CookieHandler>();
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("Web.Api.Auth"));

builder.Services.AddHttpClient<IDataService, DataService>(client =>
    {
        client.BaseAddress = new Uri("https://localhost:7062/api/data/");
    })
    .AddHttpMessageHandler<JwtTokenMessageHandler>(); 

var app = builder.Build();

await JwtTokenRefresher.RefreshJwtToken(app);
await app.RunAsync();

