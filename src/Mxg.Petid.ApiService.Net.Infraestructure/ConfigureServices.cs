// =======================================================================================
// Description: Configure services for the infraestructure layer.
// Author:      Alfredo Estrada
// Date:        2025-01-04
// Version:     1.0.0
// =======================================================================================

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Mxg.Petid.ApiService.Net.Application.Common.Interfaces.Authentication;
using Mxg.Petid.ApiService.Net.Application.Common.Interfaces.Infraestructure;
using Mxg.Petid.ApiService.Net.Application.Common.Interfaces.Persistance;
using Mxg.Petid.ApiService.Net.Application.Common.Interfaces.Security;
using Mxg.Petid.ApiService.Net.Application.Common.Models.Email;
using Mxg.Petid.ApiService.Net.Application.Common.Models.Identity;
using Mxg.Petid.ApiService.Net.Infraestructure.Persistance;
using Mxg.Petid.ApiService.Net.Infraestructure.Persistance.Repositories;
using Mxg.Petid.ApiService.Net.Infraestructure.Services;
using Mxg.Petid.ApiService.Net.Infraestructure.Services.Email;
using Mxg.Petid.ApiService.Net.Infraestructure.Services.Encrypt;
using System.Text;

namespace Mxg.Petid.ApiService.Net.Infraestructure;

/// <summary>
/// Configure infraestructure services.
/// </summary>
public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration["MxgPetid_ConnectionString"];
        var jwtSettingsKey = configuration["MxgPetid_JwtSettingsKey"];
        var senderGridApiKey = configuration["MxgPetid_SenderGridApiKey"];

        services.AddDbContext<PetidDbContext>(options => options.UseSqlServer(connectionString));

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped(typeof(IAsyncRepository<>), typeof(RepositoryBase<>));

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ICollectionTypeRepository, CollectionTypeRepository>();

        services.Configure<EmailSettings>(emailSettings =>
        {
            configuration.GetSection("EmailSettings").Bind(emailSettings);
            emailSettings.ApiKey = senderGridApiKey!;
        });
        services.AddTransient<IEmailService, SendGridEmailService>();

        services.AddTransient<IEncryptSevice, BCryptEncryptSevice>();

        services.AddHttpContextAccessor();

        services.AddTransient<IJwtProviderService, JwtProviderService>();

        services.Configure<JwtSettings>(jwtSettings =>
        {
            configuration.GetSection("JwtSettings").Bind(jwtSettings);
            jwtSettings.Key = jwtSettingsKey!;
        });

        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettingsKey!)),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            RequireExpirationTime = false,
            ClockSkew = TimeSpan.Zero,
        };

        services.AddSingleton(tokenValidationParameters);

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.SaveToken = true;
            options.TokenValidationParameters = tokenValidationParameters;
        });

        return services;
    }
}