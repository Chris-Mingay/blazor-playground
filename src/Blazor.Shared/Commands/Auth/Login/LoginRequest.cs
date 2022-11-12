using System.ComponentModel.DataAnnotations;

namespace Blazor.Shared.Commands.Auth.Login;

public class LoginRequest
{
    [Required]
    [EmailAddress]
    public string EmailAddress { get; set; }
    [Required]
    public string Password { get; set; }
}