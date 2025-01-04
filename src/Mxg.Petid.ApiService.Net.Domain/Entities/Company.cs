using Mxg.Petid.ApiService.Net.Domain.Common;

namespace Mxg.Petid.ApiService.Net.Domain.Entities;

/// <summary>
/// Company entity.
/// </summary>
public class Company : BaseAuditableEntity
{
    public Company()
    {
        IdentificationCode = string.Empty;
        Name = string.Empty;
        Users = new HashSet<User>();
    }

    public string IdentificationCode { get; set; }
    public string Name { get; set; }
    public string? Address { get; set; }
    public Guid? OwnerUserId { get; set; }
    public Guid? CityId { get; set; }

    public virtual User? OwnerUser { get; set; }
    public virtual City? City { get; set; }
    public virtual ICollection<User> Users { get; set; }
}