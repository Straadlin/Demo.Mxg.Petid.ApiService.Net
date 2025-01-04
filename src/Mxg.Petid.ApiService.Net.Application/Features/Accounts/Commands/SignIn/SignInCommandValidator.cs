using FluentValidation;

namespace Mxg.Petid.ApiService.Net.Application.Features.Accounts.Commands.SignIn;

/// <summary>
/// Rules to validate SignInCommand.
/// </summary>
public class SignInCommandValidator : AbstractValidator<SignInCommand>
{
    public SignInCommandValidator()
    {
        RuleFor(p => p.Password)
            .NotEmpty().WithMessage("{Password} is neccesary.")
            .NotNull().WithMessage("{Password} is neccesary.")
            .MinimumLength(8)
            .MaximumLength(100);

        RuleFor(p => p.Username)
            .NotEmpty().WithMessage("{Username} is neccesary when {Email} and {PhoneNumber} are also null or empty.")
            .NotNull().WithMessage("{Username} is neccesary when {Email} and {PhoneNumber} are also null or empty.")
            .MinimumLength(12)
            .MaximumLength(12)
            .When(p => string.IsNullOrWhiteSpace(p.Email) && string.IsNullOrEmpty(p.PhoneNumber)).WithMessage("{Username} cannot be null or empty when {Email} and {PhoneNumber} are also null or empty.");

        RuleFor(p => p.Email)
            .NotEmpty().WithMessage("{Email} is neccesary when {Username} and {PhoneNumber} are also null or empty.")
            .NotNull().WithMessage("{Email} is neccesary when {Username} and {PhoneNumber} are also null or empty.")
            .MinimumLength(5)
            .MaximumLength(100)
            .When(p => string.IsNullOrEmpty(p.Username) && string.IsNullOrEmpty(p.PhoneNumber)).WithMessage("{Email} cannot be null or empty when {Username} and {PhoneNumber} are also null or empty.");

        RuleFor(p => p.PhoneNumber)
            .NotEmpty().WithMessage("{PhoneNumber} is neccesary when {Username} and {Email} are also null or empty.")
            .NotNull().WithMessage("{PhoneNumber} is neccesary when {Username} and {Email} are also null or empty.")
            .MinimumLength(8)
            .MaximumLength(13)
            .When(p => string.IsNullOrEmpty(p.Username) && string.IsNullOrEmpty(p.Email)).WithMessage("{PhoneNumber} cannot be null or empty when {Username} and {Email} are also null or empty.");
    }
}