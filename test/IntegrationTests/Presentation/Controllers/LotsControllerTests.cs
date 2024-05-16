using Application.App.Lots.Responses;
using AutoMapper;
using Infrastructure.Persistance.Repositories;
using IntegrationTests.Helpers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
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

        var servicesProvider = TestHelpers.GetServiceProvider(entityRepository);

        var mediator = servicesProvider.GetRequiredService<IMediator>();

        var mapper = servicesProvider.GetRequiredService<IMapper>();

        var controller = new LotsController(mediator, mapper);

        var lotId = 1;

        var resultRequest = await controller.GetLot(lotId);

        var result = resultRequest.Result as OkObjectResult;
        var dto = result!.Value as LotDto;

        Assert.NotNull(dto);
        Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
    }
}
