using Application.Abstractions;
using Application.App.Commands.Auctions;
using Infrastructure.Persistance;
using Infrastructure.Persistance.Repositories;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

var diContainer = new ServiceCollection()
    .AddSingleton<AuctionAppDbContext, AuctionAppDbContext>()
    .AddSingleton<IRepository, EntityRepository>()
    .AddSingleton<IUnitOfWork, UnitOfWork>()
    .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(IUnitOfWork).Assembly))
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