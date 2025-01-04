using MediatR;
using Mxg.Petid.ApiService.Net.Application.Features.Accounts.Commands.SignIn.Dtos;
using System.Text.Json.Serialization;

namespace Mxg.Petid.ApiService.Net.Application.Features.Accounts.Commands.SignIn;

/// <summary>
/// Body of the sign in request.
/// </summary>
public class SignInCommand : IRequest<SignInDto>
{
    public string? Username { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string Password { get; set; } = string.Empty;

    [JsonIgnore]
    public string IpAddress { get; set; } = string.Empty;
}