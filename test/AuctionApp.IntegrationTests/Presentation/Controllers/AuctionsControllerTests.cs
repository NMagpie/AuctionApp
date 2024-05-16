using Application.App.Auctions.Responses;
using AutoMapper;
using Infrastructure.Persistance.Repositories;
using IntegrationTests.Helpers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Presentation.Controllers;
using System.Net;

namespace IntegrationTests.Presentation.Controllers;
public class AuctionsControllerTests
{
    [Fact]
    public async void AuctionsController_GetAuctionGood()
    {
        var nAuctions = 1;
        var nLots = 3;
        var nUsers = 1;

        using var contextBuilder = new DataContextBuilder();

        contextBuilder.SeedUsers(nUsers);
        contextBuilder.SeedAuctions(nAuctions);
        contextBuilder.SeedLots(nLots);

        var dbContext = contextBuilder.GetContext();

        var entityRepository = new EntityRepository(dbContext);

        var serviceProvider = TestHelpers.GetServiceProvider(entityRepository);

        var mediator = serviceProvider.GetRequiredService<IMediator>();

        var mapper = serviceProvider.GetRequiredService<IMapper>();

        var controller = new AuctionsController(mediator, mapper);

        var auctionId = 1;

        var resultRequest = await controller.GetAuction(auctionId);

        var result = resultRequest.Result as OkObjectResult;
        var dto = result!.Value as AuctionDto;

        Assert.NotNull(dto);
        Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
    }
}
