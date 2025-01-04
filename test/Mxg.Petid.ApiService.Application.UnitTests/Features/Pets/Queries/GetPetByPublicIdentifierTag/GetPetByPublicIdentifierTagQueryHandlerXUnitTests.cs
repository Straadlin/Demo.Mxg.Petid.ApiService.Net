using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using Mxg.Petid.ApiService.Application.UnitTests.Mocks;
using Mxg.Petid.ApiService.Net.Application.Common.Mappings;
using Mxg.Petid.ApiService.Net.Application.Features.Pets.Queries.GetPetByPublicIdentifierTag;
using Mxg.Petid.ApiService.Net.Application.Features.Pets.Queries.GetPetByPublicIdentifierTag.Dtos;
using Mxg.Petid.ApiService.Net.Infraestructure.Persistance.Repositories;
using Shouldly;

namespace Mxg.Petid.ApiService.Application.UnitTests.Features.Pets.Queries.GetPetByPublicIdentifierTag;

public class GetPetByPublicIdentifierTagQueryHandlerXUnitTests
{
    private readonly IMapper mapper;
    private readonly Mock<UnitOfWork> unitOfWork;

    public GetPetByPublicIdentifierTagQueryHandlerXUnitTests()
    {
        this.unitOfWork = MockUnitOfWork.GetUnitOfWork();

        var mapperConfig = new MapperConfiguration(c =>
        {
            c.AddProfile<MappingProfile>();
        });
        this.mapper = mapperConfig.CreateMapper();

        MockPetRepository.AddDataRepository(unitOfWork.Object.PetidDbContext);
    }

    [Fact]
    public async Task GetPetByPublicIdentifierTag()
    {
        // Arrange
        var handler = new GetPetByPublicIdentifierTagQueryHandler(unitOfWork.Object, this.mapper);
        var request = new GetPetByPublicIdentifierTagQuery("35EDDE5C578F4471A4C94C68324675E9");

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        result.ShouldBeOfType<GetPetByPublicIdentifierTagDto>();
        result.ShouldNotBeNull();
    }
}