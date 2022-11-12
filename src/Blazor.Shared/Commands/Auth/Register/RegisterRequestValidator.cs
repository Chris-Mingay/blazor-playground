using FluentValidation;

namespace Blazor.Shared.Commands.Auth.Register;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.DisplayName)
            .NotEmpty()
            .MinimumLength(5)
            .WithMessage("Display name must be at least 5 characters");

        RuleFor(x => x.EmailAddress)
            .EmailAddress()
            .WithMessage("Email address must be valid");

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(8)
            .WithMessage("Password must be at least 8 characters");
    }
}