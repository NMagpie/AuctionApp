using Application;
using AuctionApp.Presentation.Common.Configurations;
using AuctionApp.Presentation.SignalR;
using Infrastructure;
using Microsoft.AspNetCore.SignalR;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Reflection;

namespace Presentation;
public static class DependencyInjection
{
    public static void AddServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddInfrastructureServices(builder.Configuration);

        builder.Services.AddApplicationServices();

        var corsSettings = builder
            .Configuration
            .GetSection("CorsSettings")
            .Get<CorsSettings>();

        builder.Services
            .AddCors(options =>
            {
                options.AddPolicy("ReactAppPolicy", builder =>
                {
                    builder
                        .WithOrigins(corsSettings.AllowedOrigin)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                });
            })
            .AddControllers();

        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSingleton<ExceptionHandlingHubFilter>();

        builder.Services
            .AddSignalR(options => options.AddFilter<ExceptionHandlingHubFilter>());

        builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

        builder.Services.AddSwaggerGen(option =>
        {
            option.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Name = "Authorization",
                Scheme = "bearer",
                Type = SecuritySchemeType.Http,
            });

            option.OperationFilter<SecurityRequirementsOperationFilter>();

            option.EnableAnnotations();
        });
    }
}
