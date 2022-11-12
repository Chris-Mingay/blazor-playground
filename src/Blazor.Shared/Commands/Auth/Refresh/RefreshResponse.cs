namespace Blazor.Shared.Commands.Auth.Refresh;

public class RefreshResponse
{
    public bool Success { get; init; }
    public string AccessToken { get; init; }

    public RefreshResponse(bool success, string accessToken)
    {
        Success = success;
        AccessToken = accessToken;
    }
}