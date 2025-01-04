using Microsoft.AspNetCore.Authorization;

namespace Mxg.Petid.ApiService.Net.Application.Common.Models.Authentication;

public class PermissionRequirement : IAuthorizationRequirement
{
    public string Permissions { get; }

    public PermissionRequirement(string permissions)
    {
        Permissions = permissions;
    }
}