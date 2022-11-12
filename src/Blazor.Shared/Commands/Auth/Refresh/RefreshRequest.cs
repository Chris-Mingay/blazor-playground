namespace Blazor.Shared.Commands.Auth.Refresh;

public class RefreshRequest
{
    public string AccessToken { get; init; }

    public RefreshRequest(string accessToken)
    {
        AccessToken = accessToken;
    }
}