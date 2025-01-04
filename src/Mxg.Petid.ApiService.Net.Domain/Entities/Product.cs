using Mxg.Petid.ApiService.Net.Domain.Common;

namespace Mxg.Petid.ApiService.Net.Domain.Entities;

/// <summary>
/// Product entity.
/// </summary>
public class Product : BaseAuditableEntity
{
    public Product()
    {
        Code = string.Empty;
        Name = string.Empty;
        Description = string.Empty;

        Resources = new HashSet<Resource>();
    }

    public string Code { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal SalePrice { get; set; }
    public decimal PurchasePrice { get; set; }

    public virtual ICollection<Resource> Resources { get; set; }
}