using Application;
using Infrastructure.Persistance;

namespace Presentation;
public static class DependencyInjection
{
    public static void AddServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddInfrastructureServices(builder.Configuration);

        builder.Services.AddApplicationServices();

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
    }
}
