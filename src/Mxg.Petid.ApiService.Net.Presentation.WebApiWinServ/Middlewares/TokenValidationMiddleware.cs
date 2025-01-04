// =======================================================================================
// Description: Middleware to check if the request and token is still valid.
// Author:      Alfredo Estrada
// Date:        2025-01-04
// Version:     1.0.0
// =======================================================================================

using MediatR;
using Mxg.Petid.ApiService.Net.Application.Common.Interfaces.Authentication;
using Mxg.Petid.ApiService.Net.Application.Common.Interfaces.Persistance;
using Mxg.Petid.ApiService.Net.Application.Features.Accounts.Queries.ValidateIsTokenDenied;

namespace Mxg.Petid.ApiService.Net.Presentation.WebApiWinServ.Middlewares;

/// <summary>
/// Middleware to check if the request and token is still valid.
/// </summary>
public class TokenValidationMiddleware
{
    private readonly RequestDelegate next;
    private readonly ILogger<TokenValidationMiddleware> logger;
    private readonly IHostEnvironment env;

    /// <summary>
    /// Constructor to initialize values.
    /// </summary>
    /// <param name="next"></param>
    /// <param name="logger"></param>
    /// <param name="env"></param>
    /// <exception cref="ArgumentNullException">Null arguments.</exception>
    public TokenValidationMiddleware(RequestDelegate next, ILogger<TokenValidationMiddleware> logger, IHostEnvironment env)
    {
        this.next = next;
        this.logger = logger;
        this.env = env;
        this.next = next ?? throw new ArgumentNullException(nameof(next));
    }

    /// <summary>
    /// Method to invoke the middleware.
    /// </summary>
    /// <param name="context">Context service.</param>
    /// <param name="unitOfWork">Unit of work service</param>
    /// <param name="mediator">Mediator service.</param>
    /// <param name="jwtProviderService">Jwt Provider Service</param>
    /// <returns>Result of executed task.</returns>
    public async Task InvokeAsync(HttpContext context, IUnitOfWork unitOfWork, IMediator mediator, IJwtProviderService jwtProviderService)
    {
        try
        {
            // TODO - Esta logica debe ser migrada al 'AuthorizationActionHandler', sin embargo no pude relaizarlo por
            //  errores con la inyección del servicio IUnitOfWork, ya que no lo detecta a pesar de que sí está inyectado.
            //  Quizás el error es por una subdependencia que hay dentro del servicio IUnitOfWork.
            // Esto se debe validar despúes de que se validen los permisos del EP, justo ahi se debera validar si en BD
            //  el token aún está activo.

            if (context.Request?.Headers is null)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }

            // Esta condicional lo coloque solo para que no me de problemas de momento con los EP del movil, ya que ahi no
            //  he limpiado los tokens al momento de invocar un login o un refreshtoken.
            if (!new[] { "/account/sign-in", "/account/sign-up", "/account/refresh-token" }
                .Contains(context.Request.Path.ToString().ToLower()))
            {
                var authorizationBearerToken = context.Request.Headers["Authorization"].ToString();

                if (!string.IsNullOrEmpty(authorizationBearerToken) && authorizationBearerToken.Split(' ').Count() > 0)
                {
                    var authorizationToken = authorizationBearerToken.Split(" ").Last();

                    if (!string.IsNullOrEmpty(authorizationToken))
                    {
                        var sessionId = jwtProviderService.GetSessionIdFromToken(authorizationToken);

                        if (sessionId != Guid.Empty)
                        {
                            var query = new ValidateIsTokenDeniedQuery
                            {
                                SessionId = sessionId
                            };

                            var isDenied = await mediator.Send(query);

                            if (isDenied)
                            {
                                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                                return;
                            }
                        }
                    }
                }
            }

            await next(context);
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}