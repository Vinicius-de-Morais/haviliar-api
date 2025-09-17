using Haviliar.Ioc.Configurations;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Haviliar.Ioc;

public static class Ioc
{
    public static string DbConnectionString { get; set; } = string.Empty;

    public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        DbConnectionString = string.Format(configuration.GetConnectionString("DefaultConnection")!,
            configuration["DB_USER"], configuration["DB_PASSWORD"]);

        services.AddScoped<SqlConnection>(_ =>
        {
            var conn = new SqlConnection(DbConnectionString);
            conn.Open();
            return conn;
        });

        services.AddDI();
    }
}

