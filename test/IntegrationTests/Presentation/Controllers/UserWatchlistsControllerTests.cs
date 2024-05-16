using Application.App.UserWatchlists.Responses;
using Infrastructure.Persistance.Repositories;
using IntegrationTests.Helpers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Presentation.Controllers;
using System.Net;

namespace IntegrationTests.Presentation.Controllers;
public class UserWatchlistsControllerTests
{
    [Fact]
    public async void UserWatchlistsController_GetUserGood()
    {
        var nUsers = 1;
        var nAuctions = 1;
        var nUserWatchlists = 1;

        using var contextBuilder = new DataContextBuilder();

        contextBuilder.SeedUsers(nUsers);
        contextBuilder.SeedAuctions(nAuctions);
        contextBuilder.SeedUserWatchlists(nUserWatchlists);

        var dbContext = contextBuilder.GetContext();

        var entityRepository = new EntityRepository(dbContext);

        var servicesProvider = TestHelpers.GetServiceProvider(entityRepository);

        var mediator = servicesProvider.GetRequiredService<IMediator>();

        var controller = new UserWatchlistsController(mediator);

        var userWatchlistId = 1;

        var resultRequest = await controller.GetUserWatchlist(userWatchlistId);

        var result = resultRequest.Result as OkObjectResult;
        var dto = result!.Value as UserWatchlistDto;

        Assert.NotNull(dto);
        Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
    }
}
