using MediatR;

namespace Mxg.Petid.ApiService.Net.Application.Features.Commons.Queries.GetGenericIdentificationCode;

/// <summary>
/// Body of the request to get a generic code.
/// </summary>
public class GetGenericCodeQuery : IRequest<string>
{
}