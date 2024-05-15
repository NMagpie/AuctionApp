using Application.Common.Abstractions;
using Domain.Auth;
using Infrastructure.Persistance;
using Infrastructure.Persistance.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddDbContext<AuctionAppDbContext>(optionBuilder =>
            {
                optionBuilder.UseSqlServer(configuration.GetConnectionString("AuctionAppConnection"));
            });

        services
            .AddAuthorization()
            .AddIdentityApiEndpoints<User>(options =>
            {
                options.Password.RequiredLength = 8;
                options.User.RequireUniqueEmail = true;
            })
            .AddDefaultTokenProviders()
            .AddEntityFrameworkStores<AuctionAppDbContext>();

        services
            .AddScoped<IEntityRepository, EntityRepository>()
            .AddScoped<IUserRepository, UserRepository>();

        return services;
    }
}
