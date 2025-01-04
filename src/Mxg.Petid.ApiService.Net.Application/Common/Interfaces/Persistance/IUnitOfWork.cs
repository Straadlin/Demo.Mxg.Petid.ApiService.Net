using Mxg.Petid.ApiService.Net.Domain.Common;

namespace Mxg.Petid.ApiService.Net.Application.Common.Interfaces.Persistance;

public interface IUnitOfWork : IDisposable
{
    IUserRepository UserRepository { get; }
    ICollectionTypeRepository CollectionTypeRepository { get; }
    ICompanyRepository CompanyRepository { get; }

    IAsyncRepository<TEntity> Repository<TEntity>() where TEntity : BaseAuditableEntity;
    Task<int> CompleteAsync();
}