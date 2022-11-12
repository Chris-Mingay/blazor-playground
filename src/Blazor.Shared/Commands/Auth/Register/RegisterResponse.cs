namespace Blazor.Shared.Commands.Auth.Register;

public class RegisterResponse
{
    public bool Success { get; }

    public RegisterResponse(bool success)
    {
        Success = success;
    }
    
    public static RegisterResponse Failed { get; } = new RegisterResponse(false);
}