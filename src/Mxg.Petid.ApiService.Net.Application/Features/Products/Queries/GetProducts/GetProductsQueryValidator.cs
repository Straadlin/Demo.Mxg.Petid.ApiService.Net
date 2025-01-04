using FluentValidation;

namespace Mxg.Petid.ApiService.Net.Application.Features.Products.Queries.GetProducts;

/// <summary>
/// Rules to validate GetProducts query.
/// </summary>
public class GetProductsQueryValidator : AbstractValidator<GetProductsQuery>
{
    public GetProductsQueryValidator()
    {
    }
}