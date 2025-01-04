// =======================================================================================
// Description: Privides the main endpoints to work authorizations and permissions.
// Author:      Alfredo Estrada
// Date:        2025-01-04
// Version:     1.0.0
// =======================================================================================

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mxg.Petid.ApiService.Net.Application.Common.Enums;
using Mxg.Petid.ApiService.Net.Application.Common.Filters;
using Mxg.Petid.ApiService.Net.Application.Features.Accounts.Commands.DeactivateAccountBySession;
using Mxg.Petid.ApiService.Net.Application.Features.Accounts.Commands.RefreshToken;
using Mxg.Petid.ApiService.Net.Application.Features.Accounts.Commands.RefreshToken.Dtos;
using Mxg.Petid.ApiService.Net.Application.Features.Accounts.Commands.SignIn;
using Mxg.Petid.ApiService.Net.Application.Features.Accounts.Commands.SignIn.Dtos;
using Mxg.Petid.ApiService.Net.Application.Features.Accounts.Commands.SignUp;
using Mxg.Petid.ApiService.Net.Application.Features.Accounts.Commands.SignUp.Dtos;
using Mxg.Petid.ApiService.Net.Application.Features.Commons.Queries.GetGenericIdentificationCode;
using Mxg.Petid.ApiService.Net.Presentation.WebApiWinServ.Controllers.Base;

namespace Mxg.Petid.ApiService.Net.Presentation.WebApiWinServ.Controllers.V1;

/// <summary>
/// Provide endpoints to manipulate information about Accounts entities.
/// </summary>
public class AccountsController : ApiControllerV1
{
    private readonly IMediator mediator;

    /// <summary>
    /// Implementation of constructor.
    /// </summary>
    /// <param name="mediator">Mediator service.</param>
    public AccountsController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    /// <summary>
    /// Register a new user.
    /// </summary>
    /// <param name="command">It's a request body with user information.</param>
    /// <returns>Http result code with its information detail.</returns>
    [HttpPost("sign-up", Name = "SignUp")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(SignUpDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<SignUpDto>> SignUp([FromBody] SignUpCommand command)
    {
        var username = await mediator.Send(new GetGenericCodeQuery());
        command.Username = username;
        var result = await mediator.Send(command);
        return Ok(result);
    }

    /// <summary>
    /// Begin a new session, getting an access token.
    /// </summary>
    /// <param name="command">It's a request body with user information.</param>
    /// <returns>Http result code with its information detail.</returns>
    [HttpPost("sign-in", Name = "SignIn")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(SignInDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<SignInDto>> SignIn([FromBody] SignInCommand command)
    {
        var remoteAddress = HttpContext?.Connection?.RemoteIpAddress?.ToString();
        command.IpAddress = string.IsNullOrWhiteSpace(remoteAddress) || remoteAddress.Split(':').Length < 4 ? string.Empty : remoteAddress.Split(':')[3];
        var result = await mediator.Send(command);
        return Ok(result);
    }

    /// <summary>
    /// Renew an access token from a refresh token.
    /// </summary>
    /// <param name="command">It's a request body with user information.</param>
    /// <returns>Http result code with its information detail.</returns>
    [HttpPost("refresh-token", Name = "RefreshToken")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(RefreshTokenDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<RefreshTokenDto>> RefreshToken([FromBody] RefreshTokenCommand command)
    {
        var remoteAddress = HttpContext?.Connection?.RemoteIpAddress?.ToString();
        command.IpAddress = string.IsNullOrWhiteSpace(remoteAddress) || remoteAddress.Split(':').Length < 4 ? string.Empty : remoteAddress.Split(':')[3];
        var result = await mediator.Send(command);
        return Ok(result);
    }

    /// <summary>
    /// Deactivate account.
    /// </summary>
    /// <returns>Http result code with its information detail.</returns>
    [HttpPatch("deactivate-account-by-session", Name = "DeactivateAccountBySession")]
    [AuthorizeAction(new[] { PermissionEnum.DEACACCN })]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> DeactivateAccountBySession()
    {
        var command = new DeactivateAccountBySessionCommand
        {
            RequestInfo = GetRequestInfo(),
        };
        await mediator.Send(command);
        return NoContent();
    }
}