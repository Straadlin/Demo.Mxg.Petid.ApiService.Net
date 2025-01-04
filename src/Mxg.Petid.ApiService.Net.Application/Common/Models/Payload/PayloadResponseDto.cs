namespace Mxg.Petid.ApiService.Net.Application.Common.Models.Payload;

public class PayloadResponseDto<T> : PayloadBase
{
    public T? Data { get; set; }

    public PayloadResponseDto(int httpStatusCode, string? message = null, T? data = default)
        : base(true, httpStatusCode, message)
    {
        Data = data;
    }
}