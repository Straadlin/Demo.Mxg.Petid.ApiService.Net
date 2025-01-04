using Mxg.Petid.ApiService.Net.Domain.Common;

namespace Mxg.Petid.ApiService.Net.Domain.Entities;

/// <summary>
/// Collection type entity.
/// </summary>
public class CollectionType : BaseAuditableEntity
{
    public CollectionType()
    {
        Code = string.Empty;
        Description = string.Empty;
        Types = new HashSet<Type>();
    }

    public string Code { get; set; }
    public string Description { get; set; }

    public virtual ICollection<Type> Types { get; set; }
}