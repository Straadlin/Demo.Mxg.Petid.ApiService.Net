using Mxg.Petid.ApiService.Net.Domain.Common;

namespace Mxg.Petid.ApiService.Net.Domain.Entities;

/// <summary>
/// Role entity.
/// </summary>
public class Role : BaseAuditableEntity
{
    public Role()
    {
        Code = string.Empty;
        Description = string.Empty;

        RolePermissions = new HashSet<RolePermission>();
        Users = new HashSet<User>();
    }

    public string Code { get; set; }
    public string Description { get; set; }

    public virtual ICollection<RolePermission> RolePermissions { get; set; }
    public virtual ICollection<User> Users { get; set; }
}