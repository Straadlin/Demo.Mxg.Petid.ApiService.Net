using AutoMapper;
using Mxg.Petid.ApiService.Net.Application.Features.Pets.Queries.GetPetByPublicIdentifierTag.Dtos;
using Mxg.Petid.ApiService.Net.Application.Features.Products.Queries.GetProducts.Dtos;
using Mxg.Petid.ApiService.Net.Domain.Entities;

namespace Mxg.Petid.ApiService.Net.Application.Common.Mappings;

/// <summary>
/// Configures the mapping between the domain entities and the DTOs.
/// </summary>
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        #region GetPetByIdentifierPublicTagQueryHandler

        CreateMap<Pet, GetPetByPublicIdentifierTagDto>();

        CreateMap<City, GetPetByPublicIdentifierTagCityDto>();

        CreateMap<Vaccine, GetPetByPublicIdentifierTagVaccineDto>();

        CreateMap<Resource, GetPetByPublicIdentifierTagResourceDto>();

        CreateMap<Resource, GetPetByPublicIdentifierTagVaccineResourceDto>();

        CreateMap<Domain.Entities.Type, GetPetByPublicIdentifierTagGenderDto>();

        #endregion

        #region GetProductsQueryHandler

        CreateMap<Product, GetProductsDto>();
        CreateMap<Resource, GetProductsResourcesDto>();

        #endregion
    }
}