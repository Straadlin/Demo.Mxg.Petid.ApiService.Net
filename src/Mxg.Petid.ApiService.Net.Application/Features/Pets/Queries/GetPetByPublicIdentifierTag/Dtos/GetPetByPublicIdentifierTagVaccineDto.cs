namespace Mxg.Petid.ApiService.Net.Application.Features.Pets.Queries.GetPetByPublicIdentifierTag.Dtos;

/// <summary>
/// Dto for the vaccine of the pet.
/// </summary>
public class GetPetByPublicIdentifierTagVaccineDto
{
    public GetPetByPublicIdentifierTagVaccineDto()
    {
        Name = string.Empty;
        Pictures = new List<GetPetByPublicIdentifierTagVaccineResourceDto>();
    }

    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Detail { get; set; }
    public DateTime VaccineApplied { get; set; }
    public DateTime? NextVaccine { get; set; }
    public Guid PetId { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? LastModifiedDate { get; set; }

    public ICollection<GetPetByPublicIdentifierTagVaccineResourceDto> Pictures { get; set; }
}