namespace Mxg.Petid.ApiService.Net.Application.Common.Models.Security;

public class VerifyPasswordParameters
{
    public string Password { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
}