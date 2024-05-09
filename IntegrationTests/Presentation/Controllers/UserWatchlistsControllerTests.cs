using Application.App.Users.Commands;
using Application.App.Users.Responses;
using Application.App.UserWatchlists.Commands;
using Application.App.UserWatchlists.Responses;
using AuctionApp.Domain.Models;
using Infrastructure.Persistance.Repositories;
using IntegrationTests.Helpers;
using Microsoft.AspNetCore.Mvc;
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

        var mediator = TestHelpers.CreateMediator(entityRepository);

        var controller = new UserWatchlistsController(mediator);

        var userWatchlistId = 1;

        var resultRequest = await controller.GetUserWatchlist(userWatchlistId);

        var result = resultRequest.Result as OkObjectResult;
        var dto = result!.Value as UserWatchlistDto;

        Assert.NotNull(dto);
        Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
    }

    [Fact]
    public async void UserWatchlistsController_PostUserGood()
    {
        var nUsers = 1;
        var nAuctions = 1;

        using var contextBuilder = new DataContextBuilder();

        contextBuilder.SeedUsers(nUsers);
        contextBuilder.SeedAuctions(nAuctions);

        var dbContext = contextBuilder.GetContext();

        var entityRepository = new EntityRepository(dbContext);

        var mediator = TestHelpers.CreateMediator(entityRepository);

        var controller = new UserWatchlistsController(mediator);

        var command = new CreateUserWatchlistCommand
        {
            UserId = 1,
            AuctionId = 1,
        };

        var resultRequest = await controller.CreateUserWatchlist(command);

        var result = resultRequest.Result as OkObjectResult;
        var dto = result!.Value as UserWatchlistDto;

        Assert.NotNull(dto);
        Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
    }

    [Fact]
    public async void UserWatchlistsController_DeleteUserGood()
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

        var mediator = TestHelpers.CreateMediator(entityRepository);

        var controller = new UserWatchlistsController(mediator);

        var userWatchlistId = 1;

        var resultRequest = await controller.DeleteUserWatchlist(userWatchlistId);

        var result = resultRequest as OkResult;

        Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
    }
}
