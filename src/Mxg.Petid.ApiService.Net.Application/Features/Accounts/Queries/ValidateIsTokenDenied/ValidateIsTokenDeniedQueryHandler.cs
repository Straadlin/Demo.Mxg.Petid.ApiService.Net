using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Mxg.Petid.ApiService.Net.Application.Common.Interfaces.Persistance;
using Mxg.Petid.ApiService.Net.Domain.Entities;

namespace Mxg.Petid.ApiService.Net.Application.Features.Accounts.Queries.ValidateIsTokenDenied;

/// <summary>
/// Query to validate if the token is denied
/// </summary>
public class ValidateIsTokenDeniedQueryHandler : IRequestHandler<ValidateIsTokenDeniedQuery, bool>
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;
    private readonly ILogger<ValidateIsTokenDeniedQueryHandler> logger;

    public ValidateIsTokenDeniedQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<ValidateIsTokenDeniedQueryHandler> logger)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
        this.logger = logger;
    }

    public async Task<bool> Handle(ValidateIsTokenDeniedQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var existingSession = (await this.unitOfWork.Repository<Session>()
                .GetAsync(predicate: p =>
                    p.Id == request.SessionId &&
                    p.IsActive &&
                    p.User.IsActive
                , orderBy: null, includeString: "User", disableTracking: true)).SingleOrDefault();

            if (existingSession is null)
                return true;
            else
                return false;
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}