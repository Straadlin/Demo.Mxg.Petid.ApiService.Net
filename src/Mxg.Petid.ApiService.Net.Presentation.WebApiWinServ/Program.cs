// =======================================================================================
// Description: This class we define the entry point of the application, where we configure it.
// Author:      Alfredo Estrada
// Date:        2025-01-04
// Version:     1.0.0
// =======================================================================================

using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Mxg.Petid.ApiService.Net.Application;
using Mxg.Petid.ApiService.Net.Application.Common.Constants;
using Mxg.Petid.ApiService.Net.Application.Common.Handlers;
using Mxg.Petid.ApiService.Net.Application.Common.Models.Authentication;
using Mxg.Petid.ApiService.Net.Infraestructure;
using Mxg.Petid.ApiService.Net.Presentation.WebApiWinServ.Middlewares;
using Mxg.Petid.ApiService.Net.Presentation.WebApiWinServ.Miscellaneous;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;

namespace Mxg.Petid.ApiService.Net.Presentation.WebApiWinServ;

/// <summary>
/// Entry point of the application.
/// </summary>
public class Program
{
    /// <summary>
    /// Main method of the application.
    /// </summary>
    /// <param name="args">Argument list.</param>
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.WebHost.ConfigureKestrel(options =>
        {
            var kestrelConfig = builder.Configuration.GetSection(CertificateConstants.KestrelCertificate);

            if (kestrelConfig != null)
            {
                var storeName = kestrelConfig[CertificateConstants.Store];
                var storeLocation = kestrelConfig[CertificateConstants.Location];
                var subject = kestrelConfig[CertificateConstants.Subject];

                if (!string.IsNullOrEmpty(storeName) && !string.IsNullOrEmpty(storeLocation) && !string.IsNullOrEmpty(subject))
                {
                    options.ConfigureHttpsDefaults(httpsOptions =>
                    {
                        var store = new X509Store(storeName, Enum.Parse<StoreLocation>(storeLocation));
                        store.Open(OpenFlags.ReadOnly);

                        var certificate = store.Certificates
                            .Find(X509FindType.FindBySubjectName, subject, validOnly: false)
                            .OfType<X509Certificate2>()
                            .FirstOrDefault();

                        if (certificate is not null)
                        {
                            httpsOptions.ServerCertificate = certificate;
                        }
                        else
                        {
                            throw new Exception($"Didn't find a certificate with subject '{subject}' in the respository '{storeName}/{storeLocation}'.");
                        }

                        store.Close();
                    });
                }
            }
        });

        if (OperatingSystem.IsWindows())
        {
            builder.Host.UseWindowsService(options => { options.ServiceName = WindowsServiceConstants.NAME_WINDOWS_SERVICE; });
        }

        builder.Services.AddControllers(options =>
        {
            options.Conventions.Add(new SwaggerGroupByVersion());
        });

        builder.Services.AddAuthorization();

        builder.Services.AddSingleton<IAuthorizationHandler, AuthorizationActionHandler>();
        builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc(SwaggerConstants.VERSION_01, new OpenApiInfo
            {
                Title = SwaggerConstants.TITLE_DOCUMENT,
                Version = SwaggerConstants.VERSION_01,
                Description = SwaggerConstants.DESCRIPTION_DOCUMENT,
                Contact = new OpenApiContact
                {
                    Email = SwaggerConstants.EMAIL_OWNER,
                    Name = SwaggerConstants.NAME_OWNER,
                    Url = new Uri(SwaggerConstants.URL_OWNER)
                }
            });

            c.AddSecurityDefinition(SecurityConstants.BEARER, new OpenApiSecurityScheme
            {
                Name = SecurityConstants.AUTHORIZATION,
                Type = SecuritySchemeType.ApiKey,
                Scheme = SecurityConstants.BEARER,
                BearerFormat = SecurityConstants.JWT,
                In = ParameterLocation.Header
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = SecurityConstants.BEARER
                        }
                    },
                    new string[]{}
                }
            });

            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath);
        });

        builder.Services.AddInfrastructureServices(builder.Configuration);
        builder.Services.AddApplicationServices();

        builder.Services.AddCors(options =>
        {
            options.AddPolicy(SecurityConstants.CORS_POLICY, builder => builder.AllowAnyOrigin()
                                                              .AllowAnyMethod()
                                                              .AllowAnyHeader());
        });

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint(SwaggerConstants.FILE_JSON_URI_V1, SwaggerConstants.FILE_URI_NAME_V1);
            });
        }

        app.UseHttpsRedirection();

        app.UseMiddleware<TokenValidationMiddleware>();
        app.UseMiddleware<ExceptionMiddleware>();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseCors(SecurityConstants.CORS_POLICY);

        app.MapControllers();

        app.Run();
    }
}