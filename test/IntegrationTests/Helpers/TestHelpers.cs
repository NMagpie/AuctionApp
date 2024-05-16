using Application.Common.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace IntegrationTests.Helpers;

public static class TestHelpers
{
    public static ServiceProvider GetServiceProvider(IEntityRepository repository)
    {
        var services = new ServiceCollection();

        services
            .AddLogging()
            .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetAssembly(typeof(IEntityRepository))))
            .AddAutoMapper(typeof(IEntityRepository))
            .AddSingleton(repository);

        var serviceProvider = services.BuildServiceProvider();
        return serviceProvider;
    }
}

