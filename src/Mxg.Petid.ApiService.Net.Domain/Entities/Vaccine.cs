using Mxg.Petid.ApiService.Net.Domain.Common;

namespace Mxg.Petid.ApiService.Net.Domain.Entities;

/// <summary>
/// Vaccine entity.
/// </summary>
public class Vaccine : BaseAuditableEntity
{
    public Vaccine()
    {
        Name = string.Empty;
        Resources = new HashSet<Resource>();
    }

    public string Name { get; set; }
    public string? Detail { get; set; }
    public DateTime VaccineApplied { get; set; }
    public DateTime? NextVaccine { get; set; }
    public Guid PetId { get; set; }
    public Guid? AppliedByCompanyId { get; set; }

    public virtual Pet? Pet { get; set; }
    public virtual ICollection<Resource> Resources { get; set; }
}