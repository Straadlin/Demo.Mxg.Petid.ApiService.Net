// =======================================================================================
// Description: Configure services for the application layer.
// Author:      Alfredo Estrada
// Date:        2025-01-04
// Version:     1.0.0
// =======================================================================================

using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Mxg.Petid.ApiService.Net.Application.Common.Behaviours;
using System.Reflection;

namespace Mxg.Petid.ApiService.Net.Application;

/// <summary>
/// Configure application services.
/// </summary>
public static class ConfigureServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

        return services;
    }
}