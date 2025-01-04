using Mxg.Petid.ApiService.Net.Domain.Common;

namespace Mxg.Petid.ApiService.Net.Domain.Entities;

/// <summary>
/// Pet entity.
/// </summary>
public class Pet : BaseAuditableEntity
{
    public Pet()
    {
        Vaccines = new HashSet<Vaccine>();
        Resources = new HashSet<Resource>();
    }

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
    public Guid? GenderTypeId { get; set; }
    public Guid? SpecieTypeId { get; set; }
    public Guid IdentifierTagId { get; set; }
    public Guid? CityId { get; set; }

    public virtual Type? GenderType { get; set; }
    public virtual Type? SpecieType { get; set; }
    public virtual IdentifierTag? IdentifierTag { get; set; }
    public virtual City? City { get; set; }
    public virtual ICollection<Vaccine> Vaccines { get; set; }
    public virtual ICollection<Resource> Resources { get; set; }
}