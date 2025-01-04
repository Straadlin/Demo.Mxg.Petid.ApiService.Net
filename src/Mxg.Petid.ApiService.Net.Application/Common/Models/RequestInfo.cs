namespace Mxg.Petid.ApiService.Net.Application.Common.Models;

public class RequestInfo
{
    public string Token { get; set; } = string.Empty;
    public Guid ClaimSessionId { get; set; }
}