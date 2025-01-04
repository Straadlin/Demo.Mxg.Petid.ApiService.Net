using Mxg.Petid.ApiService.Net.Domain.Common;

namespace Mxg.Petid.ApiService.Net.Domain.Entities;

/// <summary>
/// Country entity.
/// </summary>
public class Country : BaseAuditableEntity
{
    public Country()
    {
        Code = string.Empty;
        Name = string.Empty;
        States = new HashSet<State>();
    }

    public string Code { get; set; }
    public string Name { get; set; }

    public virtual ICollection<State> States { get; set; }
}