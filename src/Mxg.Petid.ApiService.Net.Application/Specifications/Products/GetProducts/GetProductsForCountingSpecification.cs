using Mxg.Petid.ApiService.Net.Domain.Entities;

namespace Mxg.Petid.ApiService.Net.Application.Specifications.Products.GetProducts;

/// <summary>
/// Specification for getting counting of products.
/// </summary>
public class GetProductsForCountingSpecification : BaseSpecification<Product>
{
    public GetProductsForCountingSpecification(GetProductsSpecificationParams parameters) :
        base(e =>
             e.IsActive &&
            (string.IsNullOrEmpty(parameters.Search) || e.Code!.Contains(parameters.Search))
            )
    {
    }
}