namespace Blazor.Shared.Commands.Auth.Login;

public class LoginResponse
{
    public bool Success { get; init; }
    public string AccessToken { get; init; }

    public LoginResponse(bool success, string accessToken)
    {
        Success = success;
        AccessToken = accessToken;
    }

    public static LoginResponse Failed { get; } = new LoginResponse(false, "");
}