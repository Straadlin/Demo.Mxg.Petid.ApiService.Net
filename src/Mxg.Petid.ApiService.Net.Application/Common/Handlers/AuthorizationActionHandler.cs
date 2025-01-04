using Microsoft.AspNetCore.Authorization;
using Mxg.Petid.ApiService.Net.Application.Common.Constants;
using Mxg.Petid.ApiService.Net.Application.Common.Models.Authentication;

namespace Mxg.Petid.ApiService.Net.Application.Common.Handlers;

/// <summary>
/// Check whether the user has the required permissions.
/// </summary>
public class AuthorizationActionHandler : AuthorizationHandler<PermissionRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        string? sessionId = context.User.Claims
            .FirstOrDefault(x =>
                x.Type.Contains(CustomClaimTypesConstants.SessionId)
            )?.Value;

        if (sessionId is null)
            return Task.CompletedTask;

        HashSet<string> permissionsInClaims = context.User.Claims
            .Where(x =>
                x.Type == CustomClaimTypesConstants.Permission
            ).Select(x => x.Value).ToHashSet();

        var permissionsInRequirement = requirement.Permissions.Split(',');

        if (!permissionsInRequirement.Any())
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }

        if (!permissionsInClaims.Any())
            return Task.CompletedTask;

        foreach (var permissioninRquirement in permissionsInRequirement)
        {
            if (permissionsInClaims.Contains(permissioninRquirement))
            {
                context.Succeed(requirement);
                break;
            }
        }

        return Task.CompletedTask;
    }
}