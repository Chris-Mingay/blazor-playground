using System.Security.Claims;

namespace Blazor.App;

public class LoginUser
{
    public string DisplayName { get; init; }
    public string AccessToken { get; init; }
    public ClaimsPrincipal Principal { get; init; }

    public LoginUser(string displayName, string accessToken, ClaimsPrincipal principal)
    {
        DisplayName = displayName;
        AccessToken = accessToken;
        Principal = principal;
    }

}