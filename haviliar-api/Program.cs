using Haviliar.Ioc;
using haviliar_api.Filters;
using haviliar_api.Handlers;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.Data.SqlClient;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using System.Data;
using System.Reflection;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

Ioc.RegisterServices(builder.Services, builder.Configuration);

builder.Services.AddFluentValidationRulesToSwagger();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

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
});



var app = builder.Build();
app.UseExceptionHandler();

app.Use(async (context, next) =>
{
    await next();
    var conn = context.RequestServices.GetService<SqlConnection>();
    if (conn?.State == ConnectionState.Open)
        await conn.CloseAsync();
});


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
