namespace Mxg.Petid.ApiService.Net.Application.Common.Models.Identity;

public class JwtSettings
{
    public string Key { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public TimeSpan TokenExpireTime { get; set; }
    public int MonthsToRefreshToken { get; set; }
}