using Application.Common.Abstractions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace IntegrationTests.Helpers;

public static class TestHelpers
{
    public static IMediator CreateMediator(IRepository repository)
    {
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetAssembly(typeof(IRepository))));
        services.AddSingleton(repository);
        services.AddAutoMapper(typeof(IRepository));

        var serviceProvider = services.BuildServiceProvider();
        return serviceProvider.GetRequiredService<IMediator>();
    }
}

