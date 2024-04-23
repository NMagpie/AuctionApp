using Application.Abstractions;
using Application.App.Auctions.Commands;
using Infrastructure.Persistance;
using Infrastructure.Persistance.Repositories;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

var diContainer = new ServiceCollection()
    .AddSingleton<AuctionAppDbContext, AuctionAppDbContext>()
    .AddSingleton<IRepository, EntityRepository>()
    .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(IRepository).Assembly))
    .BuildServiceProvider();

var mediator = diContainer.GetRequiredService<IMediator>();

var auction = await mediator.Send(
    new CreateAuctionCommand
    {
        Title = "123",
        CreatorId = 1,
        StartTime = DateTime.UtcNow + TimeSpan.FromMinutes(10),
        EndTime = DateTime.UtcNow + TimeSpan.FromMinutes(20),
    }
    );