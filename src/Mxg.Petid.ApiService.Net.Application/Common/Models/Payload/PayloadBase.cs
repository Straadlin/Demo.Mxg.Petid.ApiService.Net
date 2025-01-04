namespace Mxg.Petid.ApiService.Net.Application.Common.Models.Payload;

public class PayloadBase
{
    public bool IsSuccess { get; set; }
    public string? Message { get; set; } = string.Empty;
    public int HttpStatusCode { get; set; }

    public PayloadBase(bool isSuccess, int httpStatusCode, string? message = null)
    {
        IsSuccess = isSuccess;
        HttpStatusCode = httpStatusCode;

        if (string.IsNullOrEmpty(message))
            Message = GetDefaultMessageStatusCode(httpStatusCode);
        else
            Message = message;
    }

    private string GetDefaultMessageStatusCode(int httpStatusCode)
    {
        return httpStatusCode switch
        {
            200 => "Ok",
            201 => "Created",
            202 => "Accepted",
            400 => "Request has errors with validations.",
            401 => "Don't have authorization to consume this resource.",
            404 => "Don't found that resource you need it.",
            500 => "There are error in server.",
            _ => string.Empty
        };
    }
}