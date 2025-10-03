using System.Data;
using Haviliar.Infra.Context;
using Haviliar.Ioc.Configurations;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Haviliar.Ioc;

public static class Ioc
{
    public static string DbConnectionString { get; set; } = string.Empty;

    public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        DbConnectionString = string.Format(configuration.GetConnectionString("DefaultConnection")!,
            configuration["DB_USER"], configuration["DB_PASSWORD"]);


        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(DbConnectionString);
        });

        services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        services.AddDI();
    }
}
