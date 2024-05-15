using Application.App.Users.Commands;
using Application.App.Users.Responses;
using Infrastructure.Persistance.Repositories;
using IntegrationTests.Helpers;
using Microsoft.AspNetCore.Mvc;
using Presentation.Controllers;
using System.Net;

namespace IntegrationTests.Presentation.Controllers;
public class UsersControllerTests
{
    [Fact]
    public async void UsersController_GetUserGood()
    {
        var nUsers = 1;

        using var contextBuilder = new DataContextBuilder();

        contextBuilder.SeedUsers(nUsers);

        var dbContext = contextBuilder.GetContext();

        var entityRepository = new EntityRepository(dbContext);

        var mediator = TestHelpers.CreateMediator(entityRepository);

        var controller = new UsersController(mediator);

        var userId = 1;

        var resultRequest = await controller.GetUser(userId);

        var result = resultRequest.Result as OkObjectResult;
        var dto = result!.Value as UserDto;

        Assert.NotNull(dto);
        Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
    }

    [Fact]
    public async void UsersController_AddUserBalanceGood()
    {
        var nUsers = 1;

        using var contextBuilder = new DataContextBuilder();

        contextBuilder.SeedUsers(nUsers);

        var dbContext = contextBuilder.GetContext();

        var entityRepository = new EntityRepository(dbContext);

        var mediator = TestHelpers.CreateMediator(entityRepository);

        var controller = new UsersController(mediator);

        var command = new AddUserBalanceCommand
        {
            Id = 1,
            Amount = 4.99m,
        };

        var resultRequest = await controller.AddUserBalance(command);

        var result = resultRequest as OkResult;

        Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
    }

    [Fact]
    public async void UsersController_PutUserGood()
    {
        var nUsers = 1;

        using var contextBuilder = new DataContextBuilder();

        contextBuilder.SeedUsers(nUsers);

        var dbContext = contextBuilder.GetContext();

        var entityRepository = new EntityRepository(dbContext);

        var mediator = TestHelpers.CreateMediator(entityRepository);

        var controller = new UsersController(mediator);

        var command = new UpdateUserCommand
        {
            Id = 1,
            UserName = "Test",
        };

        var resultRequest = await controller.UpdateUser(command);

        var result = resultRequest.Result as OkObjectResult;
        var dto = result!.Value as UserDto;

        Assert.NotNull(dto);
        Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
    }

    [Fact]
    public async void UsersController_DeleteUserGood()
    {
        var nUsers = 1;

        using var contextBuilder = new DataContextBuilder();

        contextBuilder.SeedUsers(nUsers);

        var dbContext = contextBuilder.GetContext();

        var entityRepository = new EntityRepository(dbContext);

        var mediator = TestHelpers.CreateMediator(entityRepository);

        var controller = new UsersController(mediator);

        var userId = 1;

        var resultRequest = await controller.DeleteUser(userId);

        var result = resultRequest as OkResult;

        Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
    }
}
