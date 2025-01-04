using Microsoft.EntityFrameworkCore;
using Moq;
using Mxg.Petid.ApiService.Net.Infraestructure.Persistance;
using Mxg.Petid.ApiService.Net.Infraestructure.Persistance.Repositories;

namespace Mxg.Petid.ApiService.Application.UnitTests.Mocks;

public static class MockUnitOfWork
{
    public static Mock<UnitOfWork> GetUnitOfWork()
    {
        Guid dbContext = Guid.NewGuid();

        var options = new DbContextOptionsBuilder<PetidDbContext>()
            .UseInMemoryDatabase(databaseName: $"DbContext-{dbContext}")
            .Options;

        var dbContextFake = new PetidDbContext(options);

        dbContextFake.Database.EnsureDeleted();

        var mockUnitOfWork = new Mock<UnitOfWork>(dbContextFake);

        return mockUnitOfWork;
    }
}