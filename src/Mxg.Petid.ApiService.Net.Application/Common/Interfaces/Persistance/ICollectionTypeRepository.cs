using Mxg.Petid.ApiService.Net.Domain.Entities;

namespace Mxg.Petid.ApiService.Net.Application.Common.Interfaces.Persistance;

public interface ICollectionTypeRepository : IAsyncRepository<CollectionType>
{
    Task<CollectionType> GetCollectionTypeByCode(string code);
    Task<IEnumerable<CollectionType>> GetCollectionTypesByCode(string code);
}