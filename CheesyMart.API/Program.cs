using CheesyMart.Core.Implementations;
using CheesyMart.Core.Interfaces;
using CheesyMart.Core.Validators;
using CheesyMart.Data.Context;
using CheesyMart.Infrastructure.HealthChecks;
using CheesyMart.Infrastructure.Middleware;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
const string allowAllCors = "AllowAll";

// Add services to the container.
var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

builder.Services.AddDbContext<MainDbContext>(
    options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddValidatorsFromAssemblyContaining<CheesyProductModelValidator>();
builder.Services.AddScoped<ICheesyProductService, CheesyProductService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument();

builder.Services.AddCors(options =>
{
    options.AddPolicy(allowAllCors,
        builder =>
        {
            builder.AllowAnyHeader();
            builder.AllowAnyMethod();
            builder.AllowAnyOrigin();
        });
    
});
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "CheesyMart API",
        Version = "v1"
    });
});

builder.Host
    .UseSerilog((context, configuration) =>
    {
        configuration.ReadFrom
            .Configuration(context.Configuration);});

builder.Services.AddHealthChecks()
    .AddCheck<HealthCheck>("health");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging(options =>
{
    options.MessageTemplate = "API {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} " +
                              "ms [{RemoteIpAddress}]";
    options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
    {
        var ipAddress = httpContext.Request.Headers["CF-Connecting-IP"].FirstOrDefault();
        
        diagnosticContext.Set("RemoteIpAddress", ipAddress ?? httpContext?.Connection
            ?.RemoteIpAddress?.MapToIPv4().ToString());
    };
    
});

app.UseCors(allowAllCors);

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.MapHealthChecks("/health");

app.MapControllers();

app.Run();
