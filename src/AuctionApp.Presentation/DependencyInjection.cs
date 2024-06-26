﻿using Application;
using AuctionApp.Presentation.Common.Configurations;
using AuctionApp.Presentation.SignalR;
using AuctionApp.Presentation.SignalR.Filters;
using Infrastructure;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;
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
            .AddControllers()
            .AddNewtonsoftJson(options => 
                options.SerializerSettings.Converters.Add(new StringEnumConverter())
            );

        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSingleton<ExceptionHandlingHubFilter>();

        builder.Services.AddScoped<TransactionHandlingHubFilter>();

        builder.Services
            .AddSignalR(options => 
            {
                options
                    .AddFilter<ExceptionHandlingHubFilter>();

                options
                    .AddFilter<TransactionHandlingHubFilter>();
            });

        builder.Services.TryAddEnumerable(
            ServiceDescriptor.Singleton<IPostConfigureOptions<BearerTokenOptions>,
                ConfigureBearerTokenOptions>());

        builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

        builder.Services.AddSwaggerGenNewtonsoftSupport();

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
