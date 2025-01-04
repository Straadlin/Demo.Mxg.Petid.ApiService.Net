using Mxg.Petid.ApiService.Net.Domain.Common;

namespace Mxg.Petid.ApiService.Net.Domain.Entities;

/// <summary>
/// State entity.
/// </summary>
public class State : BaseAuditableEntity
{
    public State()
    {
        Code = string.Empty;
        Name = string.Empty;
        Cities = new HashSet<City>();
    }

    public string Code { get; set; }
    public string Name { get; set; }
    public Guid CountryId { get; set; }

    public virtual Country? Country { get; set; }
    public virtual ICollection<City> Cities { get; set; }
}