namespace Mxg.Petid.ApiService.Net.Application.Common.Exceptions;

/// <summary>
/// Exception for conflict.
/// </summary>
public class ConflictException : ApplicationException
{
    public ConflictException(string message) : base(message)
    {
    }
}