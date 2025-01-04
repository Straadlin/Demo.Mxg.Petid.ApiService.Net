using Mxg.Petid.ApiService.Net.Application.Common.Interfaces.Persistance;
using Mxg.Petid.ApiService.Net.Domain.Common;
using System.Collections;

namespace Mxg.Petid.ApiService.Net.Infraestructure.Persistance.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private Hashtable repositories;
    private readonly PetidDbContext context;
    private ICollectionTypeRepository collectionTypeRepository;
    private IUserRepository userRepository;
    private ICompanyRepository companyRepository;
    //private ISessionRepository sessionRepository;

    public IUserRepository UserRepository => this.userRepository ??= new UserRepository(this.context);
    public ICollectionTypeRepository CollectionTypeRepository => this.collectionTypeRepository ??= new CollectionTypeRepository(this.context);
    public ICompanyRepository CompanyRepository => this.companyRepository ??= new CompanyRepository(this.context);
    //public ISessionRepository SessionRepository => this.sessionRepository ??= new SessionRepository(this.context);

    public PetidDbContext PetidDbContext => this.context;

    public UnitOfWork(PetidDbContext context)
    {
        this.context = context;
    }

    public async Task<int> CompleteAsync()
    {
        return await this.context.SaveChangesAsync();
    }

    public void Dispose()
    {
        this.context.Dispose();
    }

    public IAsyncRepository<TEntity> Repository<TEntity>() where TEntity : BaseAuditableEntity
    {
        if (this.repositories == null)
        {
            this.repositories = new Hashtable();
        }

        var type = typeof(TEntity).Name;

        if (!this.repositories.ContainsKey(type))
        {
            var repositoryType = typeof(RepositoryBase<>);
            var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), this.context);
            this.repositories.Add(type, repositoryInstance);
        }

        return (IAsyncRepository<TEntity>)this.repositories[type];
    }
}