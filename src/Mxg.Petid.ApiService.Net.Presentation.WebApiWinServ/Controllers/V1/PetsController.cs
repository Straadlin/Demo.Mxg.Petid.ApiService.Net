// =======================================================================================
// Description: Privides endpoints to manipulate information about Pets entities.
// Author:      Alfredo Estrada
// Date:        2025-01-04
// Version:     1.0.0
// =======================================================================================

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mxg.Petid.ApiService.Net.Application.Features.Commons.Queries.PaginationBase.Dtos;
using Mxg.Petid.ApiService.Net.Application.Features.Pets.Queries.GetPetByPublicIdentifierTag;
using Mxg.Petid.ApiService.Net.Application.Features.Pets.Queries.GetPetByPublicIdentifierTag.Dtos;
using Mxg.Petid.ApiService.Net.Presentation.WebApiWinServ.Controllers.Base;

namespace Mxg.Petid.ApiService.Net._2024.Presentation.WebApiWinServ.Controllers.V1;

/// <summary>
/// Provide endpoints to manipulate information about Pets entities.
/// </summary>
public class PetsController : ApiControllerV1
{
    private readonly IMediator mediator;

    /// <summary>
    /// Implementation of constructor.
    /// </summary>
    /// <param name="mediator">Mediator service.</param>
    public PetsController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    /// <summary>
    /// Get pet entity by its public identifier tag.
    /// </summary>
    /// <param name="publicIdentifierTag">Public identifier tag value.</param>
    /// <returns>Entity result.</returns>
    [HttpGet("by-public-identifier-tag/{publicIdentifierTag}", Name = "GetPetByPublicIdentifierTag")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(PaginationDto<GetPetByPublicIdentifierTagDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<GetPetByPublicIdentifierTagDto>> GetPetByPublicIdentifierTag([FromRoute] string publicIdentifierTag)
    {
        var query = new GetPetByPublicIdentifierTagQuery(publicIdentifierTag);
        var result = await mediator.Send(query);

        if (result is not null)
            return Ok(result);

        return NoContent();
    }
}