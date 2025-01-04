namespace Mxg.Petid.ApiService.Net.Application.Common.Models.Security;

public class HashPasswordResult
{
    public string Hash { get; set; } = string.Empty;
    public string Salt { get; set; } = string.Empty;
    public string AlgorithmCode { get; set; } = string.Empty;
}