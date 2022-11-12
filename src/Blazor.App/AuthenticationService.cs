using System.Net.Http.Json;
using Blazor.Shared.Commands.Auth.ChangePassword;
using Blazor.Shared.Commands.Auth.Login;
using Blazor.Shared.Commands.Auth.Refresh;
using Blazor.Shared.Commands.Auth.Register;

namespace Blazor.App;

public class AuthenticationService : IAuthenticationService
{
    private readonly HttpClient _httpClient;

    public AuthenticationService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public async Task<LoginResponse> Login(LoginRequest request)
    {
        await Task.Delay(3);
        var response = await _httpClient.PostAsJsonAsync("login", request);
        return await response.Content.ReadFromJsonAsync<LoginResponse>();
    }

    public async Task<RegisterResponse> Register(RegisterRequest request)
    {
        await Task.Delay(3);
        var response = await _httpClient.PostAsJsonAsync("register", request);
        return await response.Content.ReadFromJsonAsync<RegisterResponse>();
    }
    
    public async Task<ChangePasswordResponse> ChangePassword(ChangePasswordRequest request)
    {
        await Task.Delay(3);
        var response = await _httpClient.PostAsJsonAsync("changepassword", request);
        return await response.Content.ReadFromJsonAsync<ChangePasswordResponse>();
    }

    public async Task<RefreshResponse> Refresh()
    {
        var response = await _httpClient.GetFromJsonAsync<RefreshResponse>("refresh");
        return response;
    }

    public void Dispose()
    {
        _httpClient.Dispose();
    }
}