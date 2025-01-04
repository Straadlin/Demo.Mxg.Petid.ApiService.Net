using Mxg.Petid.ApiService.Net.Application.Common.Models.Security;

namespace Mxg.Petid.ApiService.Net.Application.Common.Interfaces.Security;

/// <summary>
/// Contract for the encrypt service
/// </summary>
public interface IEncryptSevice
{
    HashPasswordResult GenerateHashPassword(HashPasswordParameters parameters);
    bool Verify(VerifyPasswordParameters parameters);
}