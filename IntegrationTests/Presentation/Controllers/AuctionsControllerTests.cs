using Application.App.Auctions.Commands;
using Application.App.Auctions.Responses;
using Application.Common.Models;
using Infrastructure.Persistance.Repositories;
using IntegrationTests.Helpers;
using Microsoft.AspNetCore.Mvc;
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

        var mediator = TestHelpers.CreateMediator(entityRepository);

        var controller = new AuctionsController(mediator);

        var auctionId = 1;

        var resultRequest = await controller.GetAuction(auctionId);

        var result = resultRequest.Result as OkObjectResult;
        var dto = result!.Value as AuctionDto;

        Assert.NotNull(dto);
        Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
    }

    [Fact]
    public async void AuctionsController_PostAuctionGood()
    {
        var nUsers = 1;

        using var contextBuilder = new DataContextBuilder();

        contextBuilder.SeedUsers(nUsers);

        var dbContext = contextBuilder.GetContext();

        var entityRepository = new EntityRepository(dbContext);

        var mediator = TestHelpers.CreateMediator(entityRepository);

        var controller = new AuctionsController(mediator);

        var command = new CreateAuctionCommand
        {
            Title = "Test",
            CreatorId = 1,
            StartTime = DateTimeOffset.UtcNow + TimeSpan.FromMinutes(10),
            EndTime = DateTimeOffset.UtcNow + TimeSpan.FromMinutes(20),
            Lots = [
                new LotInAuctionDto {
                    Title = "TestLot",
                    Description = "TestDesc",
                    InitialPrice = 4.99m,
                    Categories = [
                        new CategoryInLotDto {
                            Name = "Category-2",
                        }
                        ]
                }
                ],
        };

        var resultRequest = await controller.CreateAuction(command);

        var result = resultRequest.Result as OkObjectResult;
        var dto = result!.Value as AuctionDto;

        Assert.NotNull(dto);
        Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
    }

    [Fact]
    public async void AuctionsController_PutAuctionGood()
    {
        var nAuctions = 1;
        var nUsers = 1;

        using var contextBuilder = new DataContextBuilder();

        contextBuilder.SeedUsers(nUsers);
        contextBuilder.SeedAuctions(nAuctions);

        var dbContext = contextBuilder.GetContext();

        var entityRepository = new EntityRepository(dbContext);

        var mediator = TestHelpers.CreateMediator(entityRepository);

        var controller = new AuctionsController(mediator);

        var command = new UpdateAuctionCommand
        {
            Id = 1,
            Title = "Test",
            StartTime = DateTimeOffset.UtcNow + TimeSpan.FromMinutes(10),
            EndTime = DateTimeOffset.UtcNow + TimeSpan.FromMinutes(20),
        };

        var resultRequest = await controller.UpdateAuction(command);

        var result = resultRequest.Result as OkObjectResult;
        var dto = result!.Value as AuctionDto;

        Assert.NotNull(dto);
        Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
    }

    [Fact]
    public async void AuctionsController_DeleteAuctionGood()
    {
        var nAuctions = 2;
        var nUsers = 1;

        using var contextBuilder = new DataContextBuilder();

        contextBuilder.SeedUsers(nUsers);
        contextBuilder.SeedAuctions(nAuctions);

        var dbContext = contextBuilder.GetContext();

        var entityRepository = new EntityRepository(dbContext);

        var mediator = TestHelpers.CreateMediator(entityRepository);

        var controller = new AuctionsController(mediator);

        var auctionId = 1;

        var resultRequest = await controller.DeleteAuction(auctionId);

        var result = resultRequest as OkResult;

        Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
    }
}
