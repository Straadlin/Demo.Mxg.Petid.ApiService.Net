using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using Mxg.Petid.ApiService.Application.UnitTests.Mocks;
using Mxg.Petid.ApiService.Net.Infraestructure.Persistance.Repositories;
using Mxg.Petid.ApiService.Net.Application.Common.Mappings;
using Mxg.Petid.ApiService.Net.Application.Features.Products.Queries.GetProducts;
using Mxg.Petid.ApiService.Net.Application.Features.Products.Queries.GetProducts.Dtos;
using Shouldly;
using Mxg.Petid.ApiService.Net.Application.Features.Commons.Queries.PaginationBase.Dtos;

namespace Mxg.Petid.ApiService.Application.UnitTests.Features.Products.Queries.GetProducts;

public class GetProductsQueryHandlerXUnitTests
{
    private readonly IMapper mapper;
    private readonly Mock<UnitOfWork> unitOfWork;
    private readonly Mock<ILogger<GetProductsQueryHandler>> logger;

    public GetProductsQueryHandlerXUnitTests()
    {
        this.unitOfWork = MockUnitOfWork.GetUnitOfWork();

        var mapperConfig = new MapperConfiguration(c =>
        {
            c.AddProfile<MappingProfile>();
        });
        this.mapper = mapperConfig.CreateMapper();

        this.logger = new Mock<ILogger<GetProductsQueryHandler>>();

        MockProductRepository.AddDataRepository(this.unitOfWork.Object.PetidDbContext);
    }

    [Fact]
    public async Task GetPetByPublicIdentifierTag()
    {
        // Arrange
        var handler = new GetProductsQueryHandler(this.unitOfWork.Object, this.mapper, this.logger.Object);
        var request = new GetProductsQuery();

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        result.ShouldBeOfType<PaginationDto<GetProductsDto>>();
        result.Count.ShouldBeGreaterThan(0);
    }
}