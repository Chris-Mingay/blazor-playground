using Blazor.Shared.Commands.Auth.Refresh;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace Blazor.App;

public static class JwtTokenRefresher
{
    public static async Task RefreshJwtToken(WebAssemblyHost application)
    {
        using var boostrapScope = application.Services.CreateScope();
        using var api = boostrapScope.ServiceProvider.GetRequiredService<IAuthenticationService>();
        
        Console.WriteLine("RefreshJwtToken");

        try
        {
            var refreshTokenResponse = await api.Refresh();
            if (refreshTokenResponse.Success)
            {
                Console.WriteLine("Success");
                var loginStateService =
                    boostrapScope.ServiceProvider.GetRequiredService<JwtAuthenticationStateProvider>();
                loginStateService.Login(refreshTokenResponse.AccessToken);
            }
        }
        catch
        {
            Console.WriteLine("Exception");
        }
        
    }
}