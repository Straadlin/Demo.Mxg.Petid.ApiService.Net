using Mxg.Petid.ApiService.Net.Application.Features.Pets.Queries.GetPetByPublicIdentifierTag.Dtos;

namespace Mxg.Petid.ApiService.Net.Application.Features.Products.Queries.GetProducts.Dtos;

/// <summary>
/// Dto for products.
/// </summary>
public class GetProductsDto
{
    public Guid Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal SalePrice { get; set; }
    public decimal PurchasePrice { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? LastModifiedDate { get; set; }

    public ICollection<GetProductsResourcesDto> Resources { get; set; } = new List<GetProductsResourcesDto>();
}