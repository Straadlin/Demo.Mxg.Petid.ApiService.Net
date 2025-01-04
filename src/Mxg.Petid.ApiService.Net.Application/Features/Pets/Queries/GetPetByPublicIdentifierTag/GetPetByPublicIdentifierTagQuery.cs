using MediatR;
using Mxg.Petid.ApiService.Net.Application.Features.Pets.Queries.GetPetByPublicIdentifierTag.Dtos;

namespace Mxg.Petid.ApiService.Net.Application.Features.Pets.Queries.GetPetByPublicIdentifierTag;

/// <summary>
/// Query for getting a pet by its public identifier tag.
/// </summary>
public class GetPetByPublicIdentifierTagQuery : IRequest<GetPetByPublicIdentifierTagDto?>
{
    public string PublicIdentifierTag { get; set; }

    public GetPetByPublicIdentifierTagQuery(string publicIdentifierTag)
    {
        PublicIdentifierTag = publicIdentifierTag;
    }
}