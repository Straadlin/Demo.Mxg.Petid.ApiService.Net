using Mxg.Petid.ApiService.Net.Domain.Common;

namespace Mxg.Petid.ApiService.Net.Domain.Entities;

/// <summary>
/// Role permission entity.
/// </summary>
public class RolePermission : BaseAuditableEntity
{
    public Guid RoleId { get; set; }
    public Guid PermissionId { get; set; }

    public virtual Role? Role { get; set; }
    public virtual Permission? Permission { get; set; }
}