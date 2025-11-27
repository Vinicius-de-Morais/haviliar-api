using Haviliar.Ioc;
using haviliar_api.Filters;
using haviliar_api.Handlers;
using haviliar_api.MQTT;
using haviliar_api.WebsocketHub;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Npgsql;
using System.Data;
using System.Reflection;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

Ioc.RegisterServices(builder.Services, builder.Configuration);

builder.Services.AddFluentValidationRulesToSwagger();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddSignalR();
builder.Services.AddHostedService<MqttService>();
builder.Services.AddScoped<MqttService>();

builder.Services.AddControllers(opts =>
{
    opts.Filters.Add<AsyncAutoValidationFilter>();
}).AddJsonOptions(opts =>
{
    opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{

    opt.SupportNonNullableReferenceTypes();

    opt.MapType<DateOnly>(() => new OpenApiSchema
    {
        Type = "string",
        Example = new OpenApiString("2023-10-01"),
    });

    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "Haviliar API", Version = "v1" });


    string xmlDocumentPath = Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml");

    if (File.Exists(xmlDocumentPath))
    {
        opt.IncludeXmlComments(xmlDocumentPath);
    }

    string dataTransferXmlPath = Path.Combine(AppContext.BaseDirectory, "Haviliar.DataTransfer.xml");

    if (File.Exists(dataTransferXmlPath))
    {
        opt.IncludeXmlComments(dataTransferXmlPath);
    }

    opt.AddSecurityDefinition(
        "Bearer",
        new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Exemplo: 'Bearer ' + token'",
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer",
            BearerFormat = "JWT"
        });

    opt.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            }, new List<string>()
        }
    });
});



var app = builder.Build();
app.UseExceptionHandler();

app.Use(async (context, next) =>
{
    await next();
    var conn = context.RequestServices.GetService<NpgsqlConnection>();
    if (conn?.State == ConnectionState.Open)
        await conn.CloseAsync();
});


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(corsConfig => corsConfig
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapHub<DevicesHub>("/hubs/devices");

app.MapControllers();

app.Run();
