namespace Mxg.Petid.ApiService.Net.Domain.Common;

/// <summary>
/// Base auditable entity.
/// </summary>
public class BaseAuditableEntity : BaseEntity
{
    public BaseAuditableEntity()
    {
        CreatedBy = string.Empty;
    }

    public bool InUse { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedDate { get; set; }
    public string CreatedBy { get; set; }
    public DateTime? LastModifiedDate { get; set; }
    public string? LastModifiedBy { get; set; }
    public int ModifiedCount { get; set; }
}