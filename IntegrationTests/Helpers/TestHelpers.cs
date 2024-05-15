using Application.Common.Abstractions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace IntegrationTests.Helpers;

public static class TestHelpers
{
    public static IMediator CreateMediator(IEntityRepository repository)
    {
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetAssembly(typeof(IEntityRepository))));
        services.AddSingleton(repository);
        services.AddAutoMapper(typeof(IEntityRepository));

        var serviceProvider = services.BuildServiceProvider();
        return serviceProvider.GetRequiredService<IMediator>();
    }
}

