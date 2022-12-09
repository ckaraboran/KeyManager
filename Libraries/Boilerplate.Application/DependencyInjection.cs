using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Boilerplate.Application.Behaviors;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Boilerplate.Application;

[ExcludeFromCodeCoverage]
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(Assembly.GetExecutingAssembly());
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        return services;
    }
}