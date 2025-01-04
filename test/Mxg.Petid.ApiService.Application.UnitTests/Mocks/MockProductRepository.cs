using AutoFixture;
using Mxg.Petid.ApiService.Net.Domain.Entities;
using Mxg.Petid.ApiService.Net.Infraestructure.Persistance;

namespace Mxg.Petid.ApiService.Application.UnitTests.Mocks;

public static class MockProductRepository
{
    public static void AddDataRepository(PetidDbContext dbContextFake)
    {
        var fixture = new Fixture();
        fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        var entities = fixture.Build<Product>()
            .CreateMany(4);
        dbContextFake.Products!.AddRange(entities);

        var entity = fixture.Build<Product>()
            .With(x => x.Id, Guid.Parse("88859d26-8b98-4e83-9c09-866b3335e17a"))
            .With(x => x.InUse, false)
            .With(x => x.IsActive, true)
            .With(x => x.CreatedDate, DateTime.Now)
            .With(x => x.CreatedBy, "-3")
            .Create();
        dbContextFake.Products!.AddRange(entity);

        dbContextFake.SaveChanges();
    }
}