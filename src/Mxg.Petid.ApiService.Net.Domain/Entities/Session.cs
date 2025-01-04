using Mxg.Petid.ApiService.Net.Domain.Common;

namespace Mxg.Petid.ApiService.Net.Domain.Entities;

/// <summary>
/// Session entity.
/// </summary>
public class Session : BaseAuditableEntity
{
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpire { get; set; }
    public bool RefreshTokenIsUsed { get; set; }
    public bool RefreshTokenIsRevoked { get; set; }
    public string? IpAddress { get; set; }
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
    public decimal? Altitude { get; set; }
    public Guid UserId { get; set; }

    public virtual User? User { get; set; }
}