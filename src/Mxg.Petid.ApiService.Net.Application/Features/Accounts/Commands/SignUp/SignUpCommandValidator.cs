using FluentValidation;

namespace Mxg.Petid.ApiService.Net.Application.Features.Accounts.Commands.SignUp;

/// <summary>
/// Rules to validate the SignUpCommand.
/// </summary>
public class SignUpCommandValidator : AbstractValidator<SignUpCommand>
{
    public SignUpCommandValidator()
    {
        RuleFor(p => p.Email)
            .NotEmpty()
            .NotNull()
            .MinimumLength(5)
            .MaximumLength(100)
            .When(p => ((string.IsNullOrEmpty(p.Email) && string.IsNullOrEmpty(p.PhoneNumber)) || !string.IsNullOrEmpty(p.Email)))
            .WithMessage("{Email} cannot be null because {PhoneNumber} is also null or empty. And Email can not contains 0 characters, only can be null.");

        RuleFor(p => p.PhoneNumber)
            .NotEmpty()
            .NotNull()
            .MinimumLength(10)
            .MaximumLength(13)
            .When(p => (string.IsNullOrEmpty(p.PhoneNumber) && string.IsNullOrEmpty(p.Email)) || !string.IsNullOrEmpty(p.PhoneNumber))
            .WithMessage("{PhoneNumber} cannot be null because {Email} is also null or empty. And PhoneNumber can not contains 0 characters, only can be null.");

        RuleFor(p => p.Password)
            .NotEmpty()
            .NotNull()
            .MinimumLength(1)
            .MaximumLength(255);

        RuleFor(p => p.FirstName)
            .NotEmpty()
            .NotNull()
            .MinimumLength(3)
            .MaximumLength(100);

        RuleFor(p => p.LastName)
            .NotEmpty()
            .NotNull()
            .MinimumLength(3)
            .MaximumLength(100);
    }
}