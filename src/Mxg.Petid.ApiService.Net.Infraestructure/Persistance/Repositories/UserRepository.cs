using Mxg.Petid.ApiService.Net.Application.Common.Interfaces.Persistance;
using Mxg.Petid.ApiService.Net.Domain.Entities;

namespace Mxg.Petid.ApiService.Net.Infraestructure.Persistance.Repositories;

public class UserRepository : RepositoryBase<User>, IUserRepository
{
    public UserRepository(PetidDbContext context) : base(context)
    {
    }
}