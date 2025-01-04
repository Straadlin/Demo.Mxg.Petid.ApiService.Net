using Mxg.Petid.ApiService.Net.Domain.Common;

namespace Mxg.Petid.ApiService.Net.Domain.Entities;

/// <summary>
/// User entity.
/// </summary>
public class User : BaseAuditableEntity
{
    public User()
    {
        Username = string.Empty;

        Sessions = new HashSet<Session>();
        Companies = new HashSet<Company>();
    }

    public string Username { get; set; }
    public string? Email { get; set; }
    public bool EmailConfirmed { get; set; }
    public string? EmailConfirmedCode { get; set; }
    public string? PhoneNumber { get; set; }
    public bool PhoneNumberConfirmed { get; set; }
    public string? PhoneNumberConfirmedCode { get; set; }
    public DateTime? Birthdate { get; set; }
    public string? PasswordHash { get; set; }
    public string? PrivateInfoJson { get; set; }
    public bool IsInfoEncrypted { get; set; }
    public Guid? GenderTypeId { get; set; }
    public Guid AlgorithmPasswordTypeId { get; set; }
    public Guid? CityId { get; set; }
    public Guid? EmployeedCompanyId { get; set; }
    public Guid StatusAccountTypeId { get; set; }
    public Guid RoleId { get; set; }

    public virtual Type? GenderType { get; set; }
    public virtual Type? AlgorithmPasswordType { get; set; }
    public virtual City? City { get; set; }
    public virtual Company? EmployeedCompany { get; set; }
    public virtual Type? StatusAccountType { get; set; }
    public virtual Role? Role { get; set; }

    public virtual ICollection<Session> Sessions { get; set; }
    public virtual ICollection<Company> Companies { get; set; }
}