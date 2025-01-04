using MediatR;
using Mxg.Petid.ApiService.Net.Application.Features.Commons.Queries.PaginationBase.Dtos;
using Mxg.Petid.ApiService.Net.Application.Features.Commons.Queries.PaginationBase;
using Mxg.Petid.ApiService.Net.Application.Features.Products.Queries.GetProducts.Dtos;

namespace Mxg.Petid.ApiService.Net.Application.Features.Products.Queries.GetProducts;

/// <summary>
/// Body of the request to get the products.
/// </summary>
public class GetProductsQuery : PaginationBaseQuery, IRequest<PaginationDto<GetProductsDto>>
{
}