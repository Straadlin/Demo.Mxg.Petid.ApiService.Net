using Mxg.Petid.ApiService.Net.Application.Features.Accounts.Commands.RefreshToken.Dtos;
using Mxg.Petid.ApiService.Net.Domain.Entities;
using System.Security.Claims;

namespace Mxg.Petid.ApiService.Net.Application.Common.Interfaces.Authentication;

public interface IJwtProviderService
{
    RefreshTokenDto ValidateCurrentTokenToRefresh(Session currentSession, string accessToken);
    RefreshTokenDto GenerateToken(Session newSession, string roleCode, IList<Claim> roleClaims);
    Guid GetSessionIdFromToken(string token);
}