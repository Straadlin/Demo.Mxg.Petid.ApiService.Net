namespace Mxg.Petid.ApiService.Net.Application.Features.Products.Queries.GetProducts.Dtos;

/// <summary>
/// Dto for get products resources.
/// </summary>
public class GetProductsResourcesDto
{
    public Guid Id { get; set; }
    public string Uri { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public DateTime? LastModifiedDate { get; set; }
}