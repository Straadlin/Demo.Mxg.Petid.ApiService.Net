namespace Mxg.Petid.ApiService.Net.Application.Common.Models.Security;

public class HashPasswordParameters
{
    public string Password { get; set; } = string.Empty;
    public string Salt { get; set; } = string.Empty;
}