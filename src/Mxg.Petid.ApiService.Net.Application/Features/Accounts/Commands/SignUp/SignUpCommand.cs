using MediatR;
using Mxg.Petid.ApiService.Net.Application.Features.Accounts.Commands.SignUp.Dtos;
using System.Text.Json.Serialization;

namespace Mxg.Petid.ApiService.Net.Application.Features.Accounts.Commands.SignUp;

/// <summary>
/// Body of the sign up request.
/// </summary>
public class SignUpCommand : IRequest<SignUpDto>
{
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string Password { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime? Birthdate { get; set; }
    public Guid? GenderTypeId { get; set; }
    public Guid? CityId { get; set; }

    [JsonIgnore]
    public string Username { get; set; } = string.Empty;
}