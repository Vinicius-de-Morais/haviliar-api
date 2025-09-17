using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Haviliar.Application;
using Haviliar.Domain;
using Haviliar.Infra;
using Microsoft.Extensions.DependencyInjection;

namespace Haviliar.Ioc.Configurations;

public static class DIConfiguration
{
    public static void AddDI(this IServiceCollection services)
    {
        services.Scan(scan =>
        {
            scan.FromAssemblyOf<InfraContext>()
                .AddClasses()
                .AsImplementedInterfaces()
                .WithScopedLifetime();

            scan.FromAssemblyOf<DomainContext>()
                .AddClasses(c =>
                {
                    c.Where(type => !typeof(Exception).IsAssignableFrom(type));
                })
                .AsImplementedInterfaces()
                .WithScopedLifetime();

            scan.FromAssemblyOf<ApplicationContext>()
                .AddClasses()
                .AsImplementedInterfaces()
                .WithScopedLifetime();
        });
    }
}

