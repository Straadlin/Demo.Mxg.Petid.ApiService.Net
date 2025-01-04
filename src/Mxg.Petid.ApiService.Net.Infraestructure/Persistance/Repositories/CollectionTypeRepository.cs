using Microsoft.EntityFrameworkCore;
using Mxg.Petid.ApiService.Net.Application.Common.Interfaces.Persistance;
using Mxg.Petid.ApiService.Net.Domain.Entities;

namespace Mxg.Petid.ApiService.Net.Infraestructure.Persistance.Repositories;

public class CollectionTypeRepository : RepositoryBase<CollectionType>, ICollectionTypeRepository
{
    public CollectionTypeRepository(PetidDbContext context) : base(context)
    {

    }

    public async Task<CollectionType> GetCollectionTypeByCode(string code)
    {
        return await this.context.CollectionTypes!.Where(p => p.Code == code && p.IsActive).SingleOrDefaultAsync();
    }

    public async Task<IEnumerable<CollectionType>> GetCollectionTypesByCode(string code)
    {
        return await this.context!.CollectionTypes!.Where(v => v.Code == code && v.IsActive).ToListAsync();
    }
}