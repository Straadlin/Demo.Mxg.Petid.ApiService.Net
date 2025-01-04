using FluentValidation;

namespace Mxg.Petid.ApiService.Net.Application.Features.Accounts.Commands.DeactivateAccountBySession;

/// <summary>
/// Rules to validate the DeactivateAccountBySessionCommand.
/// </summary>
public class DeactivateAccountBySessionCommandValidator : AbstractValidator<DeactivateAccountBySessionCommand>
{
    public DeactivateAccountBySessionCommandValidator()
    {
    }
}