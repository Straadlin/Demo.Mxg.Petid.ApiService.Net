using AutoFixture;
using Mxg.Petid.ApiService.Net.Domain.Entities;
using Mxg.Petid.ApiService.Net.Infraestructure.Persistance;

namespace Mxg.Petid.ApiService.Application.UnitTests.Mocks;

public static class MockAccountRepository
{
    public static void AddDataRepository(PetidDbContext dbContextFake)
    {
        var fixture = new Fixture();
        fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        //var entities = fixture.Build<IdentifierTag>()
        //    .CreateMany(1);
        //dbContextFake.IdentifierTags!.AddRange(entities);

        var entity = fixture.Build<User>()
            .With(x => x.Id, Guid.Parse("ed4f23d4-afb6-445a-8a8b-ed28f7f518c3"))
            .With(x => x.Username, "52CA97585A464BD7BDF4A88158CF9EB3")
            .With(x => x.Email, "prueba123@prueba.com")
            .With(x => x.PasswordHash, "$2a$11$IdFpWVh2ac4HuXnUOh.gcOpcbVA7NpGGZDc2ikIbc/tOK.65JCssO")
            .With(x => x.PrivateInfoJson, "eEG04IAaEr3TPkptaGDIyto/0oN15aRMwfMC+szat/VAqsiSW6SHnLgKUE9rTCdJBiJ8uGLZy8UeSnabz8uBqFY/lfM6jeFc76n9LxVYlaIRJb8baCoRY7cT0cu2r9wYvKg/eSGQKyqBqFAmeXAkQmMJC74uFaUVdSUSteGiUdlhoWt9Q2q7xmUuLzfpYpzDl52G1eKwDv0Sb5dFALwARKmrV+Szpnb4fiWf2u8+epAWqVOKRkAyoqRkMPQllzEz")
            .With(x => x.InUse, false)
            .With(x => x.IsActive, true)
            .With(x => x.CreatedDate, DateTime.Now)
            .With(x => x.CreatedBy, "-3")
            .Create();
        dbContextFake.Users!.Add(entity);

        dbContextFake.SaveChanges();
    }
}