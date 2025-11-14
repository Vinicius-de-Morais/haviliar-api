using System.Data;
using Haviliar.Infra.Context;
using Haviliar.Ioc.Configurations;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Haviliar.Infra.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Haviliar.Ioc;

public static class Ioc
{
    public static string DbConnectionString { get; set; } = string.Empty;

    public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {

        JwtSettings jwtSettings = new JwtSettings();

        configuration.Bind(JwtSettings.SectionName, jwtSettings);

        services.Configure<JwtSettings>(options =>
        {
            options.Issuer = jwtSettings.Issuer;
            options.Audience = jwtSettings.Audience;
            options.Key = jwtSettings.Key;
        });

        services
       .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
       .AddJwtBearer(
           (opts) =>
           {
               opts.TokenValidationParameters = new TokenValidationParameters
               {
                   ValidateIssuer = true,
                   ValidateAudience = true,
                   ValidateLifetime = true,
                   ValidateIssuerSigningKey = true,
                   ValidIssuer = jwtSettings.Issuer,
                   ValidAudience = jwtSettings.Audience,
                   IssuerSigningKey = new SymmetricSecurityKey(
                       Encoding.UTF8.GetBytes(jwtSettings.Key)
                   ),
                   ClockSkew = TimeSpan.Zero,
               };
           }
       );

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
