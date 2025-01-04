using AutoFixture;
using Mxg.Petid.ApiService.Net.Domain.Entities;
using Mxg.Petid.ApiService.Net.Infraestructure.Persistance;

namespace Mxg.Petid.ApiService.Application.UnitTests.Mocks;

public static class MockPetRepository
{
    public static void AddDataRepository(PetidDbContext dbContextFake)
    {
        var fixture = new Fixture();
        fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        var entities = fixture.Build<Pet>()
            .Without(x => x.IdentifierTag)
            .CreateMany(4);
        dbContextFake.Pets!.AddRange(entities);

        var entity = fixture.Build<Pet>()
            .With(x => x.Id, Guid.Parse("b6855e87-3497-4ff0-9b4e-9c7eb9d89524"))
            .With(x => x.Name, "Name Pet")
            .With(x => x.InUse, false)
            .With(x => x.IsActive, true)
            .With(x => x.CreatedDate, DateTime.Now)
            .With(x => x.CreatedBy, "-3")
            .With(x => x.IdentifierTag,
                new IdentifierTag
                {
                    Id = Guid.Parse("2c216657-93b7-4043-b97e-a4aabf2d09f2"),
                    PublicIdentifierTag = "35EDDE5C578F4471A4C94C68324675E9",
                    InUse = false,
                    IsActive = true,
                    CreatedDate = DateTime.Now,
                    CreatedBy = "-3"
                })
            .Create();
        dbContextFake.Pets!.Add(entity);

        dbContextFake.SaveChanges();
    }
}