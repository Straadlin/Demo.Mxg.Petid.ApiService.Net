namespace Mxg.Petid.ApiService.Net.Application.Common.Exceptions;

/// <summary>
/// Exception for not found.
/// </summary>
public class NotFoundException : ApplicationException
{
    public NotFoundException(string name, object key) : base($"Entity \"{name}\" ({key}) didn't found")
    {
    }
}