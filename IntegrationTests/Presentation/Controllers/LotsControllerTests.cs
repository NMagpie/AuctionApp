using Application.App.Lots.Commands;
using Application.App.Lots.Responses;
using Infrastructure.Persistance.Repositories;
using IntegrationTests.Helpers;
using Microsoft.AspNetCore.Mvc;
using Presentation.Controllers;
using System.Net;

namespace IntegrationTests.Presentation.Controllers;
public class LotsControllerTests
{
    [Fact]
    public async void LotsController_GetLotGood()
    {
        var nAuctions = 1;
        var nLots = 1;
        var nUsers = 1;

        using var contextBuilder = new DataContextBuilder();

        contextBuilder.SeedUsers(nUsers);
        contextBuilder.SeedAuctions(nAuctions);
        contextBuilder.SeedLots(nLots);

        var dbContext = contextBuilder.GetContext();

        var entityRepository = new EntityRepository(dbContext);

        var mediator = TestHelpers.CreateMediator(entityRepository);

        var controller = new LotsController(mediator);

        var lotId = 1;

        var resultRequest = await controller.GetLot(lotId);

        var result = resultRequest.Result as OkObjectResult;
        var dto = result!.Value as LotDto;

        Assert.NotNull(dto);
        Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
    }

    [Fact]
    public async void LotsController_PostLotGood()
    {
        var nAuctions = 1;
        var nLots = 1;
        var nUsers = 1;

        using var contextBuilder = new DataContextBuilder();

        contextBuilder.SeedUsers(nUsers);
        contextBuilder.SeedAuctions(nAuctions);
        contextBuilder.SeedLots(nLots);

        var dbContext = contextBuilder.GetContext();

        var entityRepository = new EntityRepository(dbContext);

        var mediator = TestHelpers.CreateMediator(entityRepository);

        var controller = new LotsController(mediator);

        var command = new CreateLotCommand
        {
            Title = "Test",
            Description = "Test",
            AuctionId = 1,
            InitialPrice = 2.99m
        };

        var resultRequest = await controller.CreateLot(command);

        var result = resultRequest.Result as OkObjectResult;
        var dto = result!.Value as LotDto;

        Assert.NotNull(dto);
        Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
    }

    [Fact]
    public async void LotsController_PutLotGood()
    {
        var nAuctions = 1;
        var nUsers = 1;
        var nLots = 1;

        using var contextBuilder = new DataContextBuilder();

        contextBuilder.SeedUsers(nUsers);
        contextBuilder.SeedAuctions(nAuctions);
        contextBuilder.SeedLots(nLots);

        var dbContext = contextBuilder.GetContext();

        var entityRepository = new EntityRepository(dbContext);

        var mediator = TestHelpers.CreateMediator(entityRepository);

        var controller = new LotsController(mediator);

        var command = new UpdateLotCommand
        {
            Id = 1,
            Title = "Test",
            Description = "Test",
            InitialPrice = 3.99m,
        };

        var resultRequest = await controller.UpdateLot(command);

        var result = resultRequest.Result as OkObjectResult;
        var dto = result!.Value as LotDto;

        Assert.NotNull(dto);
        Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
    }

    [Fact]
    public async void LotsController_DeleteLotGood()
    {
        var nAuctions = 1;
        var nUsers = 1;
        var nLots = 2;

        using var contextBuilder = new DataContextBuilder();

        contextBuilder.SeedUsers(nUsers);
        contextBuilder.SeedAuctions(nAuctions);
        contextBuilder.SeedLots(nLots);

        var dbContext = contextBuilder.GetContext();

        var entityRepository = new EntityRepository(dbContext);

        var mediator = TestHelpers.CreateMediator(entityRepository);

        var controller = new LotsController(mediator);

        var lotId = 1;

        var resultRequest = await controller.DeleteLot(lotId);

        var result = resultRequest as OkResult;

        Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
    }
}
