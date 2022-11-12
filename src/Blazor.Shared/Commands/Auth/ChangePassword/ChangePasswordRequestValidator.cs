using FluentValidation;

namespace Blazor.Shared.Commands.Auth.ChangePassword;

public class ChangePasswordRequestValidator : AbstractValidator<ChangePasswordRequest>
{
    public ChangePasswordRequestValidator()
    {

        RuleFor(x => x.EmailAddress)
            .EmailAddress()
            .NotEmpty()
            .WithMessage("Email address must be valid");

        RuleFor(x => x.CurrentPassword)
            .NotEmpty()
            .WithMessage("Current Password is required");
        
        RuleFor(x => x.NewPassword)
            .NotEmpty()
            .WithMessage("New Password is required");
    }
}