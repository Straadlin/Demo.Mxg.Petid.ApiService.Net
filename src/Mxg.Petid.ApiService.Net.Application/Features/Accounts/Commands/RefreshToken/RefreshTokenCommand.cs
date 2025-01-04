using MediatR;
using Mxg.Petid.ApiService.Net.Application.Features.Accounts.Commands.RefreshToken.Dtos;
using System.Text.Json.Serialization;

namespace Mxg.Petid.ApiService.Net.Application.Features.Accounts.Commands.RefreshToken;

/// <summary>
/// Body of request to refresh token.
/// </summary>
public class RefreshTokenCommand : IRequest<RefreshTokenDto>
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;

    [JsonIgnore]
    public string IpAddress { get; set; } = string.Empty;
}