using MediatR;

namespace Mxg.Petid.ApiService.Net.Application.Features.Commons.Queries.GetGenericIdentificationCode;

/// <summary>
/// Quiery to get a generic code.
/// </summary>
public class GetGenericCodeQueryHandler : IRequestHandler<GetGenericCodeQuery, string>
{
    public async Task<string> Handle(GetGenericCodeQuery request, CancellationToken cancellationToken)
    {
        return Guid.NewGuid().ToString().Replace("-", "").ToUpper();
    }
}