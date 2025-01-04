using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Mxg.Petid.ApiService.Net.Application.Common.Constants;
using Mxg.Petid.ApiService.Net.Application.Common.Interfaces.Authentication;
using Mxg.Petid.ApiService.Net.Application.Common.Models.Identity;
using Mxg.Petid.ApiService.Net.Application.Features.Accounts.Commands.RefreshToken.Dtos;
using Mxg.Petid.ApiService.Net.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Mxg.Petid.ApiService.Net.Infraestructure.Services;

public class JwtProviderService : IJwtProviderService
{
    private readonly JwtSettings jwtSettings;
    private readonly TokenValidationParameters tokenValidationParameters;
    private readonly IHttpContextAccessor httpContextAccessor;

    public JwtProviderService(IOptions<JwtSettings> jwtSettings, TokenValidationParameters tokenValidationParameters, IHttpContextAccessor httpContextAccessor)
    {
        this.jwtSettings = jwtSettings.Value;
        this.tokenValidationParameters = tokenValidationParameters;
        this.httpContextAccessor = httpContextAccessor;
    }

    #region Public Implementation

    public RefreshTokenDto ValidateCurrentTokenToRefresh(Session currentSession, string accessToken)
    {
        var jwtTokenHandler = new JwtSecurityTokenHandler();
        var tokenValidationParamsClone = this.tokenValidationParameters.Clone();
        tokenValidationParamsClone.ValidateLifetime = false;

        try
        {
            // validation: El formato del Token es correcto
            var tokenVerification = jwtTokenHandler.ValidateToken(accessToken, tokenValidationParamsClone, out var validatedToken);
            //var tokenVerification = jwtTokenHandler.ValidateToken(currentSession.AccessToken, tokenValidationParamsClone, out var validatedToken);

            // validation: Verifica encriptación
            if (validatedToken is JwtSecurityToken jwtSecurityToken)
            {
                var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);

                if (result == false)
                {
                    return new RefreshTokenDto
                    {
                        Success = false,
                        Errors = new List<string>
                        {
                            "Token has encrypt errors."
                        }
                    };
                }
            }

            // validation: Verificar fecha de expiracion
            var utcExpiryDate = long.Parse(tokenVerification.Claims
                .First(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

            var expiryDate = UnixTimeStampToDateTime(utcExpiryDate);

            if (expiryDate.AddMinutes(-2) > DateTime.UtcNow)
            {
                return new RefreshTokenDto
                {
                    Success = false,
                    Errors = new List<string>
                    {
                        "El Token no ha expirado"
                    }
                };
            }

            //validation: El refresh token exista en la base de datos
            //var storedToken = await this.presLagIdentityDbContext.RefreshTokens!.FirstOrDefaultAsync(x => x.Token == request.RefreshToken);

            //if (storedToken is null)
            //{
            //    return new RefreshTokenDto
            //    {
            //        Success = false,
            //        Errors = new List<string>
            //        {
            //            "El Token no existe"
            //        }
            //    };
            //}

            // validation para verificar si el token ya fue usado

            if (currentSession.RefreshTokenIsUsed)
            {
                return new RefreshTokenDto
                {
                    Success = false,
                    Errors = new List<string>
                    {
                        "El Token ya fue usado"
                    }
                };
            }

            //validation el token fue revocado??
            if (currentSession.RefreshTokenIsRevoked)
            {
                return new RefreshTokenDto
                {
                    Success = false,
                    Errors = new List<string>
                    {
                        "El Token ha sido revocado"
                    }
                };
            }

            // Lo deshabilite solo porque actualmente ya almaceno tanto el token como el Refresh Token en la misma tabla y al 
            //  investigar note que este valor está de más, ya que se suele usar para saber si hay que revocar o no un token, en base
            //  a este campo. Pero para este fin ya existe una columna llamada REFRESH_TOKEN_IS_REVOKED
            //validar el id del token
            //var jti = tokenVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

            //if (storedToken.JwtId != jti)
            //{
            //    return new RefreshTokenDto
            //    {
            //        Success = false,
            //        Errors = new List<string>
            //        {
            //            "El token no concuerda con el valor inicial"
            //        }
            //    };
            //}

            // segunda validacion para fecha de expiracion
            if (currentSession.RefreshTokenExpire < DateTime.UtcNow)
            {
                return new RefreshTokenDto
                {
                    Success = false,
                    Errors = new List<string>
                    {
                        "El refresh token ha expirado"
                    }
                };
            }
            return new RefreshTokenDto
            {
                Success = true
            };
        }
        catch (Exception ex)
        {
            if (ex.Message.Contains("Lifetime validation failed. The token is expired"))
            {
                return new RefreshTokenDto
                {
                    Success = false,
                    Errors = new List<string>
                    {
                        "El Token ha expirado por favor tienes que volver a realizar el login"
                    }
                };
            }
            else
            {
                return new RefreshTokenDto
                {
                    Success = false,
                    Errors = new List<string>
                    {
                        "El Token tiene errores, tienes que volver a hacer el login"
                    }
                };
            }
        }
    }

    public RefreshTokenDto GenerateToken(Session newSession, string roleCode, IList<Claim> roleClaims)
    {
        try
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(this.jwtSettings.Key));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        //new Claim(JwtRegisteredClaimNames.Email, user.Email),
                        new Claim(JwtRegisteredClaimNames.Sid, newSession.Id.ToString()),
                        //new Claim(CustomClaimTypesConstants.Cid, companyId.ToString()),
                        new Claim(ClaimTypes.Role, roleCode),
                    }.Union(roleClaims)
                ),
                Issuer = this.jwtSettings.Issuer,// Este no se coloco en el curso 2 al final
                Audience = this.jwtSettings.Audience,// Este no se coloco en el curso 2 al final
                Expires = DateTime.UtcNow.Add(this.jwtSettings.TokenExpireTime),
                SigningCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = jwtTokenHandler.WriteToken(token);

            //newSession.AccessToken = jwtToken;
            newSession.RefreshTokenIsUsed = false;
            newSession.RefreshTokenIsRevoked = false;
            //ExpireDate = expires,// TODO - ¿Por qué aquí lo coloco hardcode y no lo obtuvo del JSON?. En el video lo coloco así: ExpireDate = DateTime.UtcNow.AddMonths(6)
            newSession.RefreshTokenExpire = DateTime.UtcNow.AddMonths(this.jwtSettings.MonthsToRefreshToken);//DateTime.UtcNow.AddMonths(6),
            newSession.RefreshToken = $"{GenerateRandomTokenCharacters(35)}{Guid.NewGuid().ToString().Replace("-", "").ToUpper()}";

            return new RefreshTokenDto
            {
                //AccessToken = newSession.AccessToken,
                AccessToken = jwtToken,
                RefreshToken = newSession.RefreshToken,
                Success = true,
            };
        }
        catch (Exception ex)
        {
            // TODO - Falta grabar en los logs las excepciones y relanzar la excepción.
            throw;
        }
    }

    public Guid GetSessionIdFromToken(string token)
    {
        try
        {
            var handler = new JwtSecurityTokenHandler();

            if (handler.CanReadToken(token))
            {
                var jwtToken = handler.ReadJwtToken(token);
                var claims = jwtToken.Claims;

                if (claims?.Any() == true)
                {
                    var sessionId = claims.First(x => x.Type.Contains(CustomClaimTypesConstants.SessionId)).Value;
                    return Guid.Parse(sessionId);
                }
            }
        }
        catch (Exception ex)
        {
        }

        return Guid.Empty;
    }

    #endregion

    #region Private Implementation

    private string GenerateRandomTokenCharacters(int length)
    {
        var random = new Random();
        var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length).Select(x => x[random.Next(x.Length)]).ToArray());
    }

    private DateTime UnixTimeStampToDateTime(long unixTimeStamp)
    {
        var dateTimeval = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        dateTimeval = dateTimeval.AddSeconds(unixTimeStamp).ToUniversalTime();
        return dateTimeval;
    }

    #endregion
}