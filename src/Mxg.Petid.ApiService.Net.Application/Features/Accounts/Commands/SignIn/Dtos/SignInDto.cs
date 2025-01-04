namespace Mxg.Petid.ApiService.Net.Application.Features.Accounts.Commands.SignIn.Dtos;

/// <summary>
/// Dto for sign in response.
/// </summary>
public class SignInDto
{
    public string Username { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
}