using MediatR;

namespace Mxg.Petid.ApiService.Net.Application.Features.Accounts.Queries.ValidateIsTokenDenied;

/// <summary>
/// Body of the request to validate if the token is denied.
/// </summary>
public class ValidateIsTokenDeniedQuery : IRequest<bool>
{
    public Guid SessionId { get; set; }
}