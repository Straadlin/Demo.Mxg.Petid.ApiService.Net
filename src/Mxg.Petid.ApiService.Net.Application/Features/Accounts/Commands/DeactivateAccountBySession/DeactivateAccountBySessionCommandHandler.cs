using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Mxg.Petid.ApiService.Net.Application.Common.Exceptions;
using Mxg.Petid.ApiService.Net.Application.Common.Interfaces.Persistance;
using Mxg.Petid.ApiService.Net.Domain.Entities;

namespace Mxg.Petid.ApiService.Net.Application.Features.Accounts.Commands.DeactivateAccountBySession;

/// <summary>
/// Command to deactivate an account by a session.
/// </summary>
public class DeactivateAccountBySessionCommandHandler : IRequestHandler<DeactivateAccountBySessionCommand, Unit>
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;
    private readonly ILogger<DeactivateAccountBySessionCommandHandler> logger;

    public DeactivateAccountBySessionCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<DeactivateAccountBySessionCommandHandler> logger)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
        this.logger = logger;
    }

    public async Task<Unit> Handle(DeactivateAccountBySessionCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var sessionFound = (await this.unitOfWork.Repository<Session>()
                .GetAsync(
                    predicate: s =>
                        s.Id == request.RequestInfo.ClaimSessionId &&
                        s.IsActive,
                    includeString: "User"
                )).SingleOrDefault();

            if (sessionFound?.User is null)
            {
                string message = "There is not a active valid session with same Id in their claims.";
                this.logger.LogError(message);
                throw new BadRequestException(message);
            }

            var userFound = (await this.unitOfWork.Repository<User>()
                .GetAsync(predicate: u =>
                    u.Id == sessionFound.User.Id &&
                    u.IsActive
                )).SingleOrDefault();

            if (userFound is null)
            {
                string message = "There is not a user with same Id.";
                this.logger.LogError(message);
                throw new BadRequestException(message);
            }

            var collectionTypeFound = (await this.unitOfWork.Repository<CollectionType>()
                .GetAsync(predicate: u =>
                    u.Code == "STSACC" &&
                    u.IsActive
                )).SingleOrDefault();

            if (collectionTypeFound is null)
            {
                string message = "There is not the correct account collection type.";
                this.logger.LogError(message);
                throw new BadRequestException(message);
            }

            var accountTypeFound = (await this.unitOfWork.Repository<Domain.Entities.Type>()
                .GetAsync(predicate: u =>
                    u.Code == "STSADEACOW" &&
                    u.IsActive
                )).SingleOrDefault();

            if (accountTypeFound is null)
            {
                string message = "There is not the correct account type.";
                this.logger.LogError(message);
                throw new BadRequestException(message);
            }

            if (userFound.InUse)
            {
                string message = "The user is in use mode.";
                this.logger.LogError(message);
                throw new BadRequestException(message);
            }

            ApplyPatch(userFound, request, accountTypeFound);
            this.unitOfWork.Repository<User>().UpdateEntity(userFound);
            await this.unitOfWork.CompleteAsync();

            this.logger.LogInformation($"Operation was successfully UserId: '{userFound.Id}'");
            return Unit.Value;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    private void ApplyPatch(User user, DeactivateAccountBySessionCommand patchRequest, Domain.Entities.Type accountType)
    {
        user.GenderTypeId = null;
        user.Email = null;
        user.PhoneNumber = null;
        user.Birthdate = null;
        user.PrivateInfoJson = null;
        user.IsInfoEncrypted = false;
        user.GenderTypeId = null;
        user.CityId = null;
        user.StatusAccountTypeId = accountType.Id;
        user.IsActive = false;
        user.LastModifiedBy = patchRequest.RequestInfo.ClaimSessionId.ToString();
    }
}