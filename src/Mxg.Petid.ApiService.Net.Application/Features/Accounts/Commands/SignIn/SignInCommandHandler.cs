using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Mxg.Petid.ApiService.Net.Application.Common.Constants;
using Mxg.Petid.ApiService.Net.Application.Common.Exceptions;
using Mxg.Petid.ApiService.Net.Application.Common.Interfaces.Authentication;
using Mxg.Petid.ApiService.Net.Application.Common.Interfaces.Infraestructure;
using Mxg.Petid.ApiService.Net.Application.Common.Interfaces.Persistance;
using Mxg.Petid.ApiService.Net.Application.Common.Interfaces.Security;
using Mxg.Petid.ApiService.Net.Application.Common.Models;
using Mxg.Petid.ApiService.Net.Application.Common.Models.Security;
using Mxg.Petid.ApiService.Net.Application.Features.Accounts.Commands.SignIn.Dtos;
using Mxg.Petid.ApiService.Net.Domain.Entities;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace Mxg.Petid.ApiService.Net.Application.Features.Accounts.Commands.SignIn;

/// <summary>
/// Command to sign in a user.
/// </summary>
public class SignInCommandHandler : IRequestHandler<SignInCommand, SignInDto>
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;
    private readonly IEmailService emailService;
    private readonly ILogger<SignInCommandHandler> logger;
    private readonly IJwtProviderService jwtProvideService;
    private readonly IEncryptSevice encryptSevice;

    public SignInCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IEmailService emailService,
        ILogger<SignInCommandHandler> logger, IJwtProviderService jwtProviderService, IEncryptSevice encryptSevice)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
        this.emailService = emailService;
        this.logger = logger;
        this.jwtProvideService = jwtProviderService;
        this.encryptSevice = encryptSevice;
    }

    #region Public Implementation

    public async Task<SignInDto> Handle(SignInCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var existingUser = await FindUserAsync(request.Username, request.Email, request.PhoneNumber);

            if (existingUser is null)
                throw new NotFoundException(nameof(User), $"[{request.Username},{request.Email},{request.PhoneNumber}]");

            var parameters = new VerifyPasswordParameters
            {
                Password = request.Password,
                PasswordHash = existingUser.PasswordHash,
            };

            if (!this.encryptSevice.Verify(parameters))
                throw new NotFoundException(nameof(User), $"[{request.Username},{request.Email},{request.PhoneNumber}]");

            var roleAndPermissions = MapRoleAndPermissions(existingUser);

            if (roleAndPermissions.Permissions?.Any() != true)
                throw new BadRequestException($"User has not any permission.");

            var permissionClaims = new List<Claim>();

            foreach (var itemPermission in roleAndPermissions.Permissions.DistinctBy(x => x.Code))
                permissionClaims.Add(new Claim(CustomClaimTypesConstants.Permission, itemPermission.Code));

            if (!permissionClaims.Any())
                throw new BadRequestException($"There isn't any permission to this user.");

            var sessionsWereClosed = await ClosePreviousSessionsAsync(existingUser);

            if (!sessionsWereClosed)
                throw new BadRequestException($"There isn't any permission to this user.");

            var newSession = new Session
            {
                UserId = existingUser.Id,
                IpAddress = request.IpAddress,
                RefreshToken = null,
                RefreshTokenExpire = DateTime.UtcNow,
                CreatedBy = "-5",
            };

            this.unitOfWork.Repository<Session>().AddEntity(newSession);
            var result = await this.unitOfWork.CompleteAsync();

            if (result < 1)
                throw new BadRequestException($"There are poblems to add new session.");

            var newToken = this.jwtProvideService.GenerateToken(newSession, roleAndPermissions.Code, permissionClaims);

            if (!newToken.Success)
                throw new BadRequestException($"There are poblems generate a new token.");

            this.unitOfWork.Repository<Session>().UpdateEntity(newSession);

            newSession.LastModifiedBy = "-5";

            result = await this.unitOfWork.CompleteAsync();

            if (result < 1)
                throw new BadRequestException($"There are poblems to update token.");

            var privateUserInfo = DecryptPrivateInfo(request, existingUser);

            var authResponse = new SignInDto
            {
                Username = existingUser.Username,
                Email = existingUser.Email,
                FirstName = privateUserInfo.FirstName,
                LastName = privateUserInfo.LastName,
                AccessToken = newToken.AccessToken!,
                RefreshToken = newSession!.RefreshToken!,
                //Role = roleAndPermissions,
            };

            return authResponse;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    #endregion

    #region Private Implementation

    private async Task<bool> ClosePreviousSessionsAsync(User user)
    {
        try
        {
            var previousActiveSessions = (await this.unitOfWork.Repository<Session>()
                .GetAsync(predicate: s =>
                    s.UserId == user.Id &&
                    s.IsActive,
                orderBy: null, includeString: null, disableTracking: false)).ToList();

            if (previousActiveSessions?.Any() == true)
            {
                foreach (var item in previousActiveSessions)
                {
                    //item.AccessToken = null;//
                    item.RefreshToken = null;//
                    item.IsActive = false;
                    item.LastModifiedBy = "-5";
                    this.unitOfWork.Repository<Session>().UpdateEntity(item);
                }

                var result = await this.unitOfWork.CompleteAsync();

                if (result < 1)
                    return false;
            }

            return true;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    private async Task<User?> FindUserAsync(string username = null, string email = null, string phoneNumber = null)
    {
        try
        {
            // TODO - Hay que ver si se puede mejorar este mapeo para que sea automático con automaper.

            //var includes = new List<Expression<Func<User, object>>>
            //{
            //    u => u.UserRoles,
            //    u => u.UserRoles.Select(ur => ur.Role)
            //};

            //var includes = new List<string>
            //{
            //    "UserRoles",
            //    "UserRoles.Role",
            //    //"UserRoles.Role.RolePermissions",
            //};

            //var includes = new List<Expression<Func<IQueryable<User>, IIncludableQueryable<User, object>>>>
            //{
            //    q => q.Include(u => u.UserRoles)
            //          .ThenInclude(ur => ur.Role)
            //          .ThenInclude(r => r.RolePermissions)
            //          .ThenInclude(rp => rp.Permission) // Assuming you want to include Permissions in RolePermissions
            //};

            var user = (await this.unitOfWork.Repository<User>()
                .GetAsync(predicate: u =>
                    ((!string.IsNullOrEmpty(username) && u.Username == username) || (!string.IsNullOrEmpty(email) && u.Email == email) || (!string.IsNullOrEmpty(phoneNumber) && u.PhoneNumber == phoneNumber)) &&
                    u.IsActive
                , orderBy: null, includeString: null, disableTracking: true)).SingleOrDefault();

            if (user is null)
                return null;

            await GetRoleAndPermissionsAsync(user);

            return user;
        }
        catch (Exception ex)
        {
            // TODO - Falta grabar en los logs las excepciones y relanzar la excepción.
            throw;
        }
    }

    private SignInRoleDto MapRoleAndPermissions(User user) // TODO - Hay que ver si se puede mejorar este mapeo para que sea automático con automaper. Hay que cambiar este código a otra sección para reutilziar código compartido con LOGIN Y REFRESH HANDLERS..
    {
        var role = new SignInRoleDto();
        role.Code = user.Role.Code;

        foreach (var rolePermission in user.Role.RolePermissions.Where(x => x.IsActive))
        {
            role.Permissions.Add(new SignInPermissionDto
            {
                Code = rolePermission.Permission.Code
            });
        }

        return role;
    }

    private async Task GetRoleAndPermissionsAsync(User user) // Hay que cambiar este código a otra sección para reutilziar código compartido con LOGIN Y REFRESH HANDLERS..
    {
        var includes = new List<Expression<Func<Role, object>>>
        {
            r => r.RolePermissions,
        };

        user.Role = (await this.unitOfWork.Repository<Role>()
            .GetAsync(
                predicate: r =>
                    r.Id == user.RoleId &&
                    r.IsActive,
                orderBy: null,
                includes: includes,
                disableTracking: true)).SingleOrDefault();

        if (user.Role is null)
            throw new BadRequestException("Role was not found.");

        foreach (var rolePermission in user.Role.RolePermissions)
        {
            rolePermission.Permission = (await this.unitOfWork.Repository<Permission>()
                .GetAsync(
                    predicate: p =>
                        p.Id == rolePermission.PermissionId &&
                        p.IsActive,
                    orderBy: null,
                    includeString: null,
                    disableTracking: true)).SingleOrDefault();
        }
    }

    private PrivateUserInfo DecryptPrivateInfo(SignInCommand request, User user) // TODO - MIGRAR A UN SERVICIO
    {
        var privateUserInfo = Decrypt(user.PrivateInfoJson, request.Password);

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            WriteIndented = true
        };

        var deserializedInfo = JsonSerializer.Deserialize<PrivateUserInfo>(privateUserInfo, options);

        return deserializedInfo;
    }

    private static string Decrypt(string cipherText, string key) // TODO - MIGRAR A UN SERVICIO
    {
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = Encoding.UTF8.GetBytes(key.PadRight(32).Substring(0, 32));
            aesAlg.IV = aesAlg.Key.Take(16).ToArray();

            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(cipherText)))
            {
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {
                        return srDecrypt.ReadToEnd();
                    }
                }
            }
        }
    }

    #endregion
}