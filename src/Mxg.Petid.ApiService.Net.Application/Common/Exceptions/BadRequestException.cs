namespace Mxg.Petid.ApiService.Net.Application.Common.Exceptions;

/// <summary>
/// Exception for bad request.
/// </summary>
public class BadRequestException : ApplicationException
{
    public BadRequestException(string message) : base(message)
    {
    }
}