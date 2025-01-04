namespace Mxg.Petid.ApiService.Net.Application.Features.Accounts.Commands.SignUp.Dtos;

/// <summary>
/// Dto for sign up.
/// </summary>
public class SignUpDto
{
    public string Username { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public Guid? GenderTypeId { get; set; }
    public string? GenderTypeCode { get; set; }
    public Guid? CityId { get; set; }
}