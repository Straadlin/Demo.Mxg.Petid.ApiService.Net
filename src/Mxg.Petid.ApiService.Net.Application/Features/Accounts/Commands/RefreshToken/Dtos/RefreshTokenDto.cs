namespace Mxg.Petid.ApiService.Net.Application.Features.Accounts.Commands.RefreshToken.Dtos;

/// <summary>
/// Dto for refresh token response.
/// </summary>
public class RefreshTokenDto
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public bool Success { get; set; }
    public List<string>? Errors { get; set; }
}