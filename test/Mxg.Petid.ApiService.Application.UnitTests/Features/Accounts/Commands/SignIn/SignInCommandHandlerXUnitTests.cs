using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using Mxg.Petid.ApiService.Application.UnitTests.Mocks;
using Mxg.Petid.ApiService.Net.Infraestructure.Persistance.Repositories;
using Mxg.Petid.ApiService.Net.Application.Common.Mappings;
using Mxg.Petid.ApiService.Net.Application.Features.Accounts.Commands.SignIn;
using Mxg.Petid.ApiService.Net.Application.Features.Accounts.Commands.SignIn.Dtos;
using Shouldly;
using Mxg.Petid.ApiService.Net.Application.Common.Interfaces.Infraestructure;
using Mxg.Petid.ApiService.Net.Application.Common.Interfaces.Authentication;
using Mxg.Petid.ApiService.Net.Application.Common.Interfaces.Security;
using Mxg.Petid.ApiService.Net.Application.Common.Models.Security;
using Mxg.Petid.ApiService.Net.Domain.Entities;
using System.Security.Claims;
using Mxg.Petid.ApiService.Net.Application.Features.Accounts.Commands.RefreshToken.Dtos;

namespace Mxg.Petid.ApiService.Application.UnitTests.Features.Accounts.Commands.SignIn;

public class SignInCommandHandlerXUnitTests
{
    private readonly IMapper mapper;
    private readonly Mock<UnitOfWork> unitOfWork;
    private readonly Mock<ILogger<SignInCommandHandler>> logger;
    private readonly Mock<IEmailService> emailService;
    private readonly Mock<IJwtProviderService> jwtProvideService;
    private readonly Mock<IEncryptSevice> encryptSevice;

    public SignInCommandHandlerXUnitTests()
    {
        this.unitOfWork = MockUnitOfWork.GetUnitOfWork();

        var mapperConfig = new MapperConfiguration(c =>
        {
            c.AddProfile<MappingProfile>();
        });
        this.mapper = mapperConfig.CreateMapper();

        this.logger = new Mock<ILogger<SignInCommandHandler>>();
        this.emailService = new Mock<IEmailService>();
        this.jwtProvideService = new Mock<IJwtProviderService>();
        this.encryptSevice = new Mock<IEncryptSevice>();

        MockAccountRepository.AddDataRepository(this.unitOfWork.Object.PetidDbContext);
    }

    [Fact]
    public async Task SignIn_WhenEmailDoesntExistInDatabase_ShouldAddNewRegisterAndReturnObjectResult()
    {
        // Arrange

        const string accessToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJqdGkiOiI4ZGI3YWZmMS1iMzdlLTRlYzItYTQ4Yi1lZGIzMWFhOWM0OGEiLCJzaWQiOiI2MiIsInBybSI6WyJSRURST0wiLCJBRERST0wiLCJSRURQUk1TIiwiQUREUFJNUyIsIkRFQUNBQ0NOIiwiUkVEQ1VTIiwiQUREQ1VTIiwiTURZQ1VTIiwiREVMQ1VTIiwiUkVETE5EIiwiQURETE5EIiwiTURZTE5EIiwiREVMTE5EIiwiUkVEUFZEIiwiQUREUFZEIiwiTURZUFZEIiwiREVMUFZEIiwiUkVETU5MIiwiQURETU5MIiwiTURZTU5MIiwiREVMTU5MIiwiUkVEQ09NUFkiLCJBRERDT01QWSIsIk1EWUNPTVBZIiwiREVMQ09NUFkiXSwibmJmIjoxNzM2MTA0MTU1LCJleHAiOjE3MzYxMDQ3NTUsImlhdCI6MTczNjEwNDE1NSwiaXNzIjoiU3RyYWFkUHJlc3RMYWciLCJhdWQiOiJTdHJhYWRQcmVzdExhZ1VzZXJzIn0.HxCBPt1vS0Ido_1L9ycteyM6xrEgZTFegSBZ4qcp8zw";

        var handler = new SignInCommandHandler(
            this.unitOfWork.Object, 
            this.mapper, 
            this.emailService.Object,
            this.logger.Object, 
            this.jwtProvideService.Object, 
            this.encryptSevice.Object);

        var request = new SignInCommand
        {
            Username = "52CA97585A464BD7BDF4A88158CF9EB3",
            Email = "prueba@prueba.com",
            Password = "MyPassword1Admin$",
            IpAddress = "0.0.0.0",
        };

        this.encryptSevice
            .Setup(x => x.Verify(It.IsAny<VerifyPasswordParameters>()))
            .Returns(true);

        this.jwtProvideService
            .Setup(x => x.GenerateToken(It.IsAny<Session>(), It.IsAny<string>(), It.IsAny<List<Claim>>()))
            .Returns(new RefreshTokenDto
            {
                Success = true,
                AccessToken = accessToken,
            });

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<SignInDto>();
        result.AccessToken.ShouldBe(accessToken);
    }
}