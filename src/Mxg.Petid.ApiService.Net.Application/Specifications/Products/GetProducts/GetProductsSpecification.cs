using Mxg.Petid.ApiService.Net.Domain.Entities;

namespace Mxg.Petid.ApiService.Net.Application.Specifications.Products.GetProducts;

/// <summary>
/// Specification to get products.
/// </summary>
public class GetProductsSpecification : BaseSpecification<Product>
{
    public GetProductsSpecification(GetProductsSpecificationParams parameters) :
        base(e =>
             e.IsActive &&
            (string.IsNullOrEmpty(parameters.Search) || e.Code!.Contains(parameters.Search))
            )
    {
        AddIncluded(x => x.Resources);

        ApplyPaging(parameters.PageSize * (parameters.PageIndex - 1), parameters.PageSize);

        if (!string.IsNullOrEmpty(parameters.Sort))
        {
            switch (parameters.Sort)
            {
                case "IdAsc":
                    AddOrderBy(p => p.Id);
                    break;
                case "IdDesc":
                    AddOrderByDescending(p => p.Id);
                    break;
                case "CodeAsc":
                    AddOrderBy(p => p.Code);
                    break;
                case "CodeDesc":
                    AddOrderByDescending(p => p.Code);
                    break;
                case "CreatedDateAsc":
                    AddOrderBy(p => p.CreatedDate);
                    break;
                case "CreatedDateDesc":
                    AddOrderByDescending(p => p.CreatedDate);
                    break;
                default:
                    AddOrderByDescending(p => p.CreatedDate);
                    break;
            }
        }
        else
        {
            AddOrderByDescending(p => p.CreatedDate);
        }
    }
}