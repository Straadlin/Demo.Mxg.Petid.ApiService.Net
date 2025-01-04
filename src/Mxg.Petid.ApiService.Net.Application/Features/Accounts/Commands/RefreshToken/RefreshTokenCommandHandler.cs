using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Mxg.Petid.ApiService.Net.Application.Common.Constants;
using Mxg.Petid.ApiService.Net.Application.Common.Exceptions;
using Mxg.Petid.ApiService.Net.Application.Common.Interfaces.Authentication;
using Mxg.Petid.ApiService.Net.Application.Common.Interfaces.Infraestructure;
using Mxg.Petid.ApiService.Net.Application.Common.Interfaces.Persistance;
using Mxg.Petid.ApiService.Net.Application.Features.Accounts.Commands.SignIn.Dtos;
using Mxg.Petid.ApiService.Net.Application.Features.Accounts.Commands.RefreshToken.Dtos;
using Mxg.Petid.ApiService.Net.Domain.Entities;
using System.Linq.Expressions;
using System.Security.Claims;

namespace Mxg.Petid.ApiService.Net.Application.Features.Accounts.Commands.RefreshToken;

/// <summary>
/// Command to refresh token.
/// </summary>
public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, RefreshTokenDto>
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;
    private readonly IEmailService emailService;
    private readonly ILogger<RefreshTokenCommandHandler> logger;
    private readonly IJwtProviderService jwtProvideService;

    public RefreshTokenCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IEmailService emailService,
        ILogger<RefreshTokenCommandHandler> logger, IJwtProviderService jwtProviderService)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
        this.emailService = emailService;
        this.logger = logger;
        this.jwtProvideService = jwtProviderService;
    }

    #region Public Implementation

    public async Task<RefreshTokenDto> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var currentSession = await FindSessionAsync(request);

            if (currentSession is null)
                throw new BadRequestException($"Token doesn't exist.");

            var existingUser = await FindUserAsync(currentSession.User.Username, currentSession.User.Email, currentSession.User.PhoneNumber);

            if (existingUser is null)
                throw new BadRequestException($"User doesn't exist.");

            var roleAndPermissions = MapRoleAndPermissions(existingUser);

            if (roleAndPermissions.Permissions?.Any() != true)
                throw new BadRequestException($"User has not any permission.");

            var permissionClaims = new List<Claim>();
            
            foreach (var itemPermission in roleAndPermissions.Permissions.DistinctBy(x => x.Code))
            {
                permissionClaims.Add(new Claim(CustomClaimTypesConstants.Permission, itemPermission.Code));
            }

            if (!permissionClaims.Any())
                throw new BadRequestException($"There isn't any permission to this user.");

            var validate = this.jwtProvideService.ValidateCurrentTokenToRefresh(currentSession, request.AccessToken);

            if (!validate.Success)
                return validate;

            var newSession = new Session
            {
                UserId = currentSession.UserId,
                IpAddress = request.IpAddress,
                RefreshTokenExpire = new DateTime(2000, 01, 01),
                CreatedBy = currentSession.Id.ToString(),
            };

            this.unitOfWork.Repository<Session>().AddEntity(newSession);
            var result = await this.unitOfWork.CompleteAsync();

            if (result < 1)
                throw new BadRequestException($"There are poblems to create new session.");

            var newToken = this.jwtProvideService.GenerateToken(newSession, roleAndPermissions.Code, permissionClaims);

            if (!newToken.Success)
                throw new BadRequestException($"There are poblems generate a new token.");

            currentSession.RefreshToken = null;
            currentSession.RefreshTokenIsUsed = true;
            currentSession.IsActive = false;
            currentSession.LastModifiedBy = currentSession.Id.ToString();
            newSession.LastModifiedBy = newSession.Id.ToString();

            this.unitOfWork.Repository<Session>().UpdateEntity(currentSession);
            this.unitOfWork.Repository<Session>().UpdateEntity(newSession);
            result = await this.unitOfWork.CompleteAsync();

            if (result < 1)
                throw new BadRequestException($"There are poblems to update token.");

            return newToken;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    #endregion

    #region Private Implementation

    private async Task<Session> FindSessionAsync(RefreshTokenCommand request)
    {
        try
        {
            var includes = new List<Expression<Func<Session, object>>>
            {
                s => s.User,
            };

            var session = (await this.unitOfWork.Repository<Session>()
                .GetAsync(
                    predicate: p =>
                        p.RefreshToken == request.RefreshToken &&
                        p.IsActive
                    , includes: includes, disableTracking: false
                ))
                .SingleOrDefault();

            return session;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    private async Task<User?> FindUserAsync(string username = null, string email = null, string phoneNumber = null, string identificationCode = null)
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
                .GetAsync(
                    predicate: u =>
                        ((!string.IsNullOrEmpty(username) && u.Username == username) || (!string.IsNullOrEmpty(email) && u.Email == email) || (!string.IsNullOrEmpty(phoneNumber) && u.PhoneNumber == phoneNumber)) &&
                        u.IsActive,
                    orderBy: null,
                    includeString: null,
                    disableTracking: true)).SingleOrDefault();

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

    #endregion
}