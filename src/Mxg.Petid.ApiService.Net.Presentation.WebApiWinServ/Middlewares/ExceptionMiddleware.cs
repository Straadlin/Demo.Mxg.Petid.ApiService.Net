// =======================================================================================
// Description: Middleware to manipulate exceptions and format the response body.
// Author:      Alfredo Estrada
// Date:        2025-01-04
// Version:     1.0.0
// =======================================================================================

using Mxg.Petid.ApiService.Net.Application.Common.Exceptions;
using Mxg.Petid.ApiService.Net.Application.Common.Models.Payload;
using System.Net;
using System.Text;
using System.Text.Json;

namespace Mxg.Petid.ApiService.Net.Presentation.WebApiWinServ.Middlewares;

/// <summary>
/// Middleware to manipulate exceptions and format the response body.
/// </summary>
public class ExceptionMiddleware
{
    private readonly RequestDelegate next;
    private readonly ILogger<ExceptionMiddleware> logger;
    private readonly IHostEnvironment env;

    /// <summary>
    /// Constructor to initialize values.
    /// </summary>
    /// <param name="next"></param>
    /// <param name="logger"></param>
    /// <param name="env"></param>
    /// <exception cref="ArgumentNullException">Null arguments.</exception>
    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
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
    /// <returns>Result of executed task.</returns>
    public async Task InvokeAsync(HttpContext context)
    {
        var originalBodyStream = context.Response.Body;
        var memoryStream = new MemoryStream();
        context.Response.Body = memoryStream;

        try
        {
            await this.next(context);

            if (MustBeFormatResponseBody(context))
            {
                memoryStream.Seek(0, SeekOrigin.Begin);
                var responseBody = await new StreamReader(memoryStream).ReadToEndAsync();
                var modifiedResponse = FormatResponse(context.Response.StatusCode, responseBody);

                var newResponseBody = Encoding.UTF8.GetBytes(modifiedResponse);
                memoryStream.SetLength(0);
                await memoryStream.WriteAsync(newResponseBody);
                memoryStream.Seek(0, SeekOrigin.Begin);
            }

            memoryStream.Seek(0, SeekOrigin.Begin);
            await memoryStream.CopyToAsync(originalBodyStream);
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, ex.Message);

            context.Response.ContentType = "application/json";
            var httpStatusCode = (int)HttpStatusCode.InternalServerError;
            var result = string.Empty;

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };

            switch (ex)
            {
                case NotFoundException notFoundException:
                    httpStatusCode = (int)HttpStatusCode.NotFound;
                    break;
                case ValidationException validationException:
                    httpStatusCode = (int)HttpStatusCode.BadRequest;
                    var validationJson = JsonSerializer.Serialize(validationException.Errors);
                    result = JsonSerializer.Serialize(
                        new PayloadErrorExceptionResponseDto(httpStatusCode, ex.Message, validationJson),
                        options);
                    break;
                case ConflictException conflictException:
                    httpStatusCode = (int)HttpStatusCode.Conflict;
                    break;
                case BadRequestException badRequestException:
                    httpStatusCode = (int)HttpStatusCode.BadRequest;
                    break;
                default:
                    break;
            }

            if (string.IsNullOrEmpty(result))
            {
                result = JsonSerializer.Serialize(env.IsDevelopment()
                    ? new PayloadErrorExceptionResponseDto(httpStatusCode, ex.Message, ex.StackTrace)
                    : new PayloadErrorExceptionResponseDto(httpStatusCode),
                options);
            }

            context.Response.StatusCode = httpStatusCode;
            var errorResponse = Encoding.UTF8.GetBytes(result);
            memoryStream.SetLength(0);
            await memoryStream.WriteAsync(errorResponse);
            memoryStream.Seek(0, SeekOrigin.Begin);

            memoryStream.Seek(0, SeekOrigin.Begin);
            await memoryStream.CopyToAsync(originalBodyStream);
        }
        finally
        {
            context.Response.Body = originalBodyStream;
            await context.Response.Body.FlushAsync();
        }
    }

    /// <summary>
    /// Validate whether the response body must be formatted or not.
    /// </summary>
    /// <param name="context">Http Context.</param>
    /// <returns>Result.</returns>
    private bool MustBeFormatResponseBody(HttpContext context)
    {
        if (context.Response.StatusCode == StatusCodes.Status200OK ||
            context.Response.StatusCode == StatusCodes.Status201Created)
            return true;

        return false;
    }

    /// <summary>
    /// Format the response body.
    /// </summary>
    /// <param name="statusCode">Status code</param>
    /// <param name="responseBody">Response body.</param>
    /// <returns>Result.</returns>
    private string FormatResponse(int statusCode, string responseBody)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        var dataObject = JsonSerializer.Deserialize<object>(responseBody);

        var formattedResponse = JsonSerializer.Serialize(
            new PayloadResponseDto<object>(statusCode, statusCode == 200 ? "Ok" : "Created", dataObject),
            options);

        return formattedResponse;
    }
}