// =======================================================================================
// Description: Privides endpoints to manipulate information about products entities.
// Author:      Alfredo Estrada
// Date:        2025-01-04
// Version:     1.0.0
// =======================================================================================

using MediatR;
using Microsoft.AspNetCore.Mvc;
using Mxg.Petid.ApiService.Net.Application.Features.Commons.Queries.PaginationBase.Dtos;
using Mxg.Petid.ApiService.Net.Presentation.WebApiWinServ.Controllers.Base;
using Mxg.Petid.ApiService.Net.Application.Common.Enums;
using Mxg.Petid.ApiService.Net.Application.Common.Filters;
using Mxg.Petid.ApiService.Net.Application.Features.Products.Queries.GetProducts.Dtos;
using Mxg.Petid.ApiService.Net.Application.Features.Products.Queries.GetProducts;

namespace Mxg.Petid.ApiService.Net.Presentation.WebApiWinServ.Controllers.V1;

/// <summary>
/// Provide endpoints to manipulate information about Pets entities.
/// </summary>
public class ProductsController : ApiControllerV1
{
    private readonly IMediator mediator;

    /// <summary>
    /// Implementation of constructor.
    /// </summary>
    /// <param name="mediator">Mediator service.</param>
    public ProductsController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    /// <summary>
    /// Get list of products.
    /// </summary>
    /// <param name="query">Query</param>
    /// <returns>Entity result.</returns>
    [HttpGet("get-products", Name = "GetProducts")]
    [AuthorizeAction(new[] { PermissionEnum.REDPROD })]
    [ProducesResponseType(typeof(PaginationDto<GetProductsDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<PaginationDto<GetProductsDto>>> GetProducts([FromQuery] GetProductsQuery query)
    {
        var paginationResults = await mediator.Send(query);

        if (paginationResults.Any())
        {
            return Ok(paginationResults);
        }

        return NoContent();
    }
}