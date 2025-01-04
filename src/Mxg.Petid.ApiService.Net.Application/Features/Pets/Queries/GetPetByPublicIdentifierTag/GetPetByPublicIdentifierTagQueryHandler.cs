using AutoMapper;
using MediatR;
using Mxg.Petid.ApiService.Net.Application.Common.Interfaces.Persistance;
using Mxg.Petid.ApiService.Net.Application.Features.Pets.Queries.GetPetByPublicIdentifierTag.Dtos;
using Mxg.Petid.ApiService.Net.Domain.Entities;

namespace Mxg.Petid.ApiService.Net.Application.Features.Pets.Queries.GetPetByPublicIdentifierTag;

/// <summary>
/// Query to get a pet by its public identifier tag.
/// </summary>
public class GetPetByPublicIdentifierTagQueryHandler : IRequestHandler<GetPetByPublicIdentifierTagQuery, GetPetByPublicIdentifierTagDto?>
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;

    public GetPetByPublicIdentifierTagQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
    }

    public async Task<GetPetByPublicIdentifierTagDto?> Handle(GetPetByPublicIdentifierTagQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var identifierTagFound = (await this.unitOfWork.Repository<IdentifierTag>()
                .GetAsync(
                    predicate: p =>
                        p.PublicIdentifierTag == request.PublicIdentifierTag &&
                        p.IsActive,
                    orderBy: null,
                    includeString: "Pet"
                )).SingleOrDefault();

            if (identifierTagFound?.Pet is not null)
            {
                var petToReturn = this.mapper.Map<GetPetByPublicIdentifierTagDto>(identifierTagFound.Pet);
                petToReturn.PublicIdentifierTag = request.PublicIdentifierTag;

                if (identifierTagFound.Pet.CityId is not null)
                {
                    var cityFound = (await this.unitOfWork.Repository<City>()
                        .GetAsync(
                            predicate: p =>
                                p.Id == identifierTagFound.Pet.CityId &&
                                p.IsActive
                        )).SingleOrDefault();

                    if (cityFound is not null)
                    {
                        petToReturn.City = this.mapper.Map<GetPetByPublicIdentifierTagCityDto>(cityFound);
                    }
                }

                if (identifierTagFound.Pet.GenderTypeId is not null)
                {
                    var genderFound = (await this.unitOfWork.Repository<Domain.Entities.Type>()
                        .GetAsync(
                            predicate: p =>
                                p.Id == identifierTagFound.Pet.GenderTypeId &&
                                p.IsActive
                        )).SingleOrDefault();

                    if (genderFound is not null)
                    {
                        petToReturn.Gender = this.mapper.Map<GetPetByPublicIdentifierTagGenderDto>(genderFound);
                    }
                }

                List<Vaccine> vaccinesFound = (await this.unitOfWork.Repository<Vaccine>()
                    .GetAsync(
                        predicate: p =>
                            p.PetId == identifierTagFound.Pet.Id &&
                            p.IsActive
                    )).OrderByDescending(o=>o.VaccineApplied).ToList();

                if (vaccinesFound.Any())
                {
                    petToReturn.Vaccines = this.mapper.Map<List<GetPetByPublicIdentifierTagVaccineDto>>(vaccinesFound);

                    foreach (var itemVaccine in petToReturn.Vaccines)
                    {
                        List<Resource> resourcesVaccinesFound = (await this.unitOfWork.Repository<Resource>()
                            .GetAsync(
                                predicate: p =>
                                    p.VaccineId == itemVaccine.Id &&
                                    p.IsActive
                            )).ToList();

                        if (resourcesVaccinesFound.Any())
                        {
                            itemVaccine.Pictures = this.mapper.Map<List<GetPetByPublicIdentifierTagVaccineResourceDto>>(resourcesVaccinesFound);
                        }
                    }
                }

                List<Resource> resourcesFound = (await this.unitOfWork.Repository<Resource>()
                    .GetAsync(
                        predicate: p =>
                            p.PetId == identifierTagFound.Pet.Id &&
                            p.IsActive
                    )).ToList();

                if (resourcesFound.Any())
                {
                    petToReturn.Pictures = this.mapper.Map<List<GetPetByPublicIdentifierTagResourceDto>>(resourcesFound);
                }

                return petToReturn;
            }
        }
        catch (Exception ex)
        {
        }

        return null;
    }
}