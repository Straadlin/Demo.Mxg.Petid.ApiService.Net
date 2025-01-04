using Microsoft.AspNetCore.Authorization;
using Mxg.Petid.ApiService.Net.Application.Common.Enums;

namespace Mxg.Petid.ApiService.Net.Application.Common.Filters;

public class AuthorizeAction : AuthorizeAttribute
{
    public AuthorizeAction(PermissionEnum[] permission)
        : base(policy: string.Join(",", permission))
    {
    }
}