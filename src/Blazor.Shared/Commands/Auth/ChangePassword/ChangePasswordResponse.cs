namespace Blazor.Shared.Commands.Auth.ChangePassword;

public class ChangePasswordResponse
{
    public bool Success { get; }

    public ChangePasswordResponse(bool success)
    {
        Success = success;
    }
    
    public static ChangePasswordResponse Failed { get; } = new ChangePasswordResponse(false);
}