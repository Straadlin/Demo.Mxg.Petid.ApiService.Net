using MediatR;
using Mxg.Petid.ApiService.Net.Application.Common.Models;
using System.Text.Json.Serialization;

namespace Mxg.Petid.ApiService.Net.Application.Features.Accounts.Commands.DeactivateAccountBySession;

/// <summary>
/// Body of request to deactivate account by session.
/// </summary>
public class DeactivateAccountBySessionCommand : IRequest<Unit>
{
    [JsonIgnore]
    public RequestInfo RequestInfo { get; set; } = new RequestInfo();
}