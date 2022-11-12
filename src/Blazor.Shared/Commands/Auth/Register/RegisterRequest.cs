using System.ComponentModel.DataAnnotations;

namespace Blazor.Shared.Commands.Auth.Register;

public class RegisterRequest
{
    public string DisplayName { get; set; }
    
    public string EmailAddress { get; set; }
    
    public string Password { get; set; }
}