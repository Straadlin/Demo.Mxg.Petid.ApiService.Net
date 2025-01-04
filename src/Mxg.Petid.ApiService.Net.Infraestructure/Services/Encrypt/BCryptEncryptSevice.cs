using Mxg.Petid.ApiService.Net.Application.Common.Constants;
using Mxg.Petid.ApiService.Net.Application.Common.Interfaces.Security;
using Mxg.Petid.ApiService.Net.Application.Common.Models.Security;

namespace Mxg.Petid.ApiService.Net.Infraestructure.Services.Encrypt;

public class BCryptEncryptSevice : IEncryptSevice
{
    private string algorithmCode = AlgorithmTypeConstants.ALGORITHM_PASSWORD_TYPE_BCRYPT;

    public HashPasswordResult GenerateHashPassword(HashPasswordParameters parameters)
    {
        try
        {
            var passwordHash = new HashPasswordResult
            {
                Hash = BCrypt.Net.BCrypt.HashPassword(parameters.Password),
                AlgorithmCode = algorithmCode
            };

            return passwordHash;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public bool Verify(VerifyPasswordParameters parameters)
    {
        try
        {
            if (string.IsNullOrEmpty(parameters.Password) || string.IsNullOrEmpty(parameters.PasswordHash))
                return false;

            var result = BCrypt.Net.BCrypt.Verify(parameters.Password, parameters.PasswordHash);
            return result;
        }
        catch (Exception ex)
        {
        }

        return false;
    }
}