using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Mxg.Petid.ApiService.Net.Application.Common.Interfaces.Persistance;
using Mxg.Petid.ApiService.Net.Application.Features.Commons.Queries.PaginationBase.Dtos;
using Mxg.Petid.ApiService.Net.Application.Features.Products.Queries.GetProducts.Dtos;
using Mxg.Petid.ApiService.Net.Application.Specifications.Products.GetProducts;
using Mxg.Petid.ApiService.Net.Domain.Entities;

namespace Mxg.Petid.ApiService.Net.Application.Features.Products.Queries.GetProducts;

/// <summary>
/// Query to get a list of products.
/// </summary>
public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, PaginationDto<GetProductsDto>>
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;
    private readonly ILogger<GetProductsQueryHandler> logger;

    public GetProductsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetProductsQueryHandler> logger)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
        this.logger = logger;
    }

    public async Task<PaginationDto<GetProductsDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Specification Pattern + Pagination

            var specificationParams = new GetProductsSpecificationParams
            {
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Search = request.Search,
                Sort = string.Empty
            };

            var specification = new GetProductsSpecification(specificationParams);
            var resultsQuery = await this.unitOfWork.Repository<Product>().GetAllWithSpec(specification);

            var specificationCount = new GetProductsForCountingSpecification(specificationParams);
            var totalCount = await this.unitOfWork.Repository<Product>().CountAsync(specificationCount);

            var data = mapper.Map<List<GetProductsDto>>(resultsQuery);

            var totalItems = resultsQuery.Count;

            var rounded = Math.Ceiling(Convert.ToDecimal(totalCount) / Convert.ToDecimal(request.PageSize));
            var totalPages = Convert.ToInt32(rounded);

            var pagination = new PaginationDto<GetProductsDto>
            {
                Count = totalCount,
                Records = data,
                PageCount = totalPages,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
            };

            return pagination;
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}