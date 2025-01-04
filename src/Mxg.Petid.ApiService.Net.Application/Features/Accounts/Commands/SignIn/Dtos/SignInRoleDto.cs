namespace Mxg.Petid.ApiService.Net.Application.Features.Accounts.Commands.SignIn.Dtos;

/// <summary>
/// Role DTO for sign in response.
/// </summary>
public class SignInRoleDto
{
    public string Code { get; set; } = string.Empty;

    public ICollection<SignInPermissionDto> Permissions { get; set; } = new List<SignInPermissionDto>();
}