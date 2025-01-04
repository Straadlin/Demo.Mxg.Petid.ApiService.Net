using Mxg.Petid.ApiService.Net.Domain.Entities;

namespace Mxg.Petid.ApiService.Net.Application.Common.Interfaces.Persistance;

public interface ICompanyRepository : IAsyncRepository<Company>
{
    Task<Company?> GetCompanyByIdentificationCodeAsync(string identificationCode);
}