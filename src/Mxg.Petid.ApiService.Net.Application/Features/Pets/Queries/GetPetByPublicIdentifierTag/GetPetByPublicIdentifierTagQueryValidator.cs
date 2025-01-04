using FluentValidation;

namespace Mxg.Petid.ApiService.Net.Application.Features.Pets.Queries.GetPetByPublicIdentifierTag;

/// <summary>
/// Rules to validate the public identifier tag.
/// </summary>
public class GetPetByPublicIdentifierTagQueryValidator : AbstractValidator<GetPetByPublicIdentifierTagQuery>
{
    public GetPetByPublicIdentifierTagQueryValidator()
    {
        RuleFor(p => p.PublicIdentifierTag)
            .MinimumLength(32)
            .MaximumLength(32);
    }
}