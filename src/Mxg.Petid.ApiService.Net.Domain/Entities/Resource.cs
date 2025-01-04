using Mxg.Petid.ApiService.Net.Domain.Common;

namespace Mxg.Petid.ApiService.Net.Domain.Entities;

/// <summary>
/// Resource entity.
/// </summary>
public class Resource : BaseAuditableEntity
{
    public Resource()
    {
        Uri = string.Empty;
        FileName = string.Empty;
    }

    public string Uri { get; set; }
    public string FileName { get; set; }
    public Guid ExtensionFileTypeId { get; set; }
    public Guid StorageTypeId { get; set; }
    public Guid? PetId { get; set; }
    public Guid? VaccineId { get; set; }
    public Guid? ProductId { get; set; }

    public virtual Type? ExtensionFileType { get; set; }
    public virtual Type? StorageType { get; set; }
    public virtual Pet? Pet { get; set; }
    public virtual Vaccine? Vaccine { get; set; }
    public virtual Product? Product { get; set; }
}