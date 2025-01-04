using Mxg.Petid.ApiService.Net.Domain.Common;

namespace Mxg.Petid.ApiService.Net.Domain.Entities;

/// <summary>
/// Identifier tag entity.
/// </summary>
public class IdentifierTag : BaseAuditableEntity
{
    public IdentifierTag()
    {
        PublicIdentifierTag = string.Empty;
    }

    public string PublicIdentifierTag { get; set; }
    public decimal? SalePrice { get; set; }
    public decimal? PurchaseCost { get; set; }
    public Guid TagTypeId { get; set; }

    public virtual Type? TagType { get; set; }
    public virtual Pet? Pet { get; set; }
}