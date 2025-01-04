namespace Mxg.Petid.ApiService.Net.Application.Features.Pets.Queries.GetPetByPublicIdentifierTag.Dtos;

/// <summary>
/// Dto for the city.
/// </summary>
public class GetPetByPublicIdentifierTagCityDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
}