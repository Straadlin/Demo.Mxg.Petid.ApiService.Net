namespace Mxg.Petid.ApiService.Net.Application.Common.Models.Payload;

public class PayloadErrorExceptionResponseDto : PayloadBase
{
    public string? ErrorDetails { get; set; }

    public PayloadErrorExceptionResponseDto(int httpStatusCode, string? message = null, string? errorDetails = null)
        : base(false, httpStatusCode, message)
    {
        ErrorDetails = errorDetails;
    }
}