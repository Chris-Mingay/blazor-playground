 using Blazor.Shared.Commands.Auth.ChangePassword;
 using Blazor.Shared.Commands.Auth.Login;
using Blazor.Shared.Commands.Auth.Refresh;
using Blazor.Shared.Commands.Auth.Register;

namespace Blazor.App;

public interface IAuthenticationService : IDisposable
{
    Task<LoginResponse> Login(LoginRequest request);
    Task<RegisterResponse> Register(RegisterRequest request);
    Task<ChangePasswordResponse> ChangePassword(ChangePasswordRequest request);
    Task<RefreshResponse> Refresh();
}