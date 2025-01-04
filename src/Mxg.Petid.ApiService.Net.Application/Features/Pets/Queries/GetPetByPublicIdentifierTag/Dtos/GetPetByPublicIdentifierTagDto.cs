namespace Mxg.Petid.ApiService.Net.Application.Features.Pets.Queries.GetPetByPublicIdentifierTag.Dtos;

/// <summary>
/// Dto for the Public Identifier Tag query.
/// </summary>
public class GetPetByPublicIdentifierTagDto
{
    public GetPetByPublicIdentifierTagDto()
    {
        PublicIdentifierTag = string.Empty;
        Vaccines = new List<GetPetByPublicIdentifierTagVaccineDto>();
        Pictures = new List<GetPetByPublicIdentifierTagResourceDto>();
    }

    public Guid Id { get; set; }
    public string PublicIdentifierTag { get; set; }
    public string? Name { get; set; }
    public DateTime? Birthdate { get; set; }
    public string? Breed { get; set; }
    public string? Color { get; set; }
    public decimal? Weight { get; set; }
    public string? DistinctiveFeature { get; set; }
    public string? Notes { get; set; }
    public string? PublicOwner { get; set; }
    public string? PublicPhoneNumber { get; set; }
    public string? PublicEmail { get; set; }
    public string? PublicAddress { get; set; }
    public Guid? SpecieTypeId { get; set; }
    public Guid IdentifierTagId { get; set; }
    public DateTime CreatedDate { get; set; }

    public GetPetByPublicIdentifierTagCityDto? City { get; set; }
    public GetPetByPublicIdentifierTagGenderDto? Gender { get; set; }
    public ICollection<GetPetByPublicIdentifierTagVaccineDto> Vaccines { get; set; }
    public ICollection<GetPetByPublicIdentifierTagResourceDto> Pictures { get; set; }
}