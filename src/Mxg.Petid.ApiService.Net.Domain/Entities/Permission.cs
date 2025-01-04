using Mxg.Petid.ApiService.Net.Domain.Common;

namespace Mxg.Petid.ApiService.Net.Domain.Entities;

/// <summary>
/// Permission entity.
/// </summary>
public class Permission : BaseAuditableEntity
{
    public Permission()
    {
        Code = string.Empty;
        Description = string.Empty;

        RolePermissions = new HashSet<RolePermission>();
    }

    public string Code { get; set; }
    public string Description { get; set; }

    public virtual ICollection<RolePermission> RolePermissions { get; set; }
}