using Mxg.Petid.ApiService.Net.Domain.Common;

namespace Mxg.Petid.ApiService.Net.Domain.Entities;

/// <summary>
/// Cuty entity.
/// </summary>
public class City : BaseAuditableEntity
{
    public City()
    {
        Code = string.Empty;
        Name = string.Empty;
        Users = new HashSet<User>();
        Companies = new HashSet<Company>();
        Pets = new HashSet<Pet>();
    }

    public string Code { get; set; }
    public string Name { get; set; }
    public Guid StateId { get; set; }

    public virtual State? State { get; set; }
    public virtual ICollection<User> Users { get; set; }
    public virtual ICollection<Company> Companies { get; set; }
    public virtual ICollection<Pet> Pets { get; set; }
}