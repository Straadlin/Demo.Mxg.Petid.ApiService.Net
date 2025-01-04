using Microsoft.AspNetCore.Mvc;
using Mxg.Petid.ApiService.Net.Application.Common.Constants;
using Mxg.Petid.ApiService.Net.Application.Common.Models;
using System.IdentityModel.Tokens.Jwt;

namespace Mxg.Petid.ApiService.Net.Presentation.WebApiWinServ.Controllers.Base;

[ApiController]
public class ApiControllerBase : ControllerBase
{
    private protected RequestInfo GetRequestInfo()
    {
        var requestInfo = new RequestInfo();

        var authorizationHeader = HttpContext.Request.Headers[GeneralConstants.Authorization].ToString();
        var token = authorizationHeader!.Substring(GeneralConstants.BearerSpace.Length).Trim();
        requestInfo.Token = token;

        var userContext = HttpContext?.User;
        if (userContext != null && userContext.Identity != null && userContext.Identity.IsAuthenticated)
        {
            requestInfo.ClaimSessionId = Guid.Parse(userContext.Claims.SingleOrDefault(c => c.Type == JwtRegisteredClaimNames.Sid)!.Value);
        }

        return requestInfo;
    }
}