using FluentValidation;

namespace Mxg.Petid.ApiService.Net.Application.Features.Accounts.Commands.RefreshToken;

/// <summary>
/// Rules to validate refresh token.
/// </summary>
public class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
{
    public RefreshTokenCommandValidator()
    {
        RuleFor(p => p.AccessToken)
            .NotEmpty()
            .NotNull()
            .MinimumLength(5)
            .MaximumLength(2000)
            .WithMessage("{Token} cannot be null or empty.");

        RuleFor(p => p.RefreshToken)
            .NotEmpty()
            .NotNull()
            .MinimumLength(5)
            .MaximumLength(2000)
            .WithMessage("{RefreshToken} cannot be null or empty.");
    }
}