using AutoFixture;
using Mxg.Petid.ApiService.Net.Domain.Entities;
using Mxg.Petid.ApiService.Net.Infraestructure.Persistance;

namespace Mxg.Petid.ApiService.Application.UnitTests.Mocks;

public static class MockIdentificationTagRepository
{
    public static void AddDataRepository(PetidDbContext dbContextFake)
    {
        var fixture = new Fixture();
        fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        var entities = fixture.Build<IdentifierTag>()
            .CreateMany(4);
        dbContextFake.IdentifierTags!.AddRange(entities);

        var entity = fixture.Build<IdentifierTag>()
            .With(x => x.Id, Guid.Parse("b6855e87-3497-4ff0-9b4e-9c7eb9d89524"))
            .With(x => x.InUse, false)
            .With(x => x.IsActive, true)
            .With(x => x.CreatedDate, DateTime.Now)
            .With(x => x.CreatedBy, "-3")
            .Create();
        dbContextFake.IdentifierTags!.AddRange(entity);

        dbContextFake.SaveChanges();
    }
}