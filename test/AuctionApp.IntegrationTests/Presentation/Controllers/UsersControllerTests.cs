using Application.App.Users.Responses;
using AutoMapper;
using Infrastructure.Persistance.Repositories;
using IntegrationTests.Helpers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
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

        var servicesProvider = TestHelpers.GetServiceProvider(entityRepository);

        var mediator = servicesProvider.GetRequiredService<IMediator>();

        var mapper = servicesProvider.GetRequiredService<IMapper>();

        var controller = new UsersController(mediator, mapper);

        var userId = 1;

        var resultRequest = await controller.GetUser(userId);

        var result = resultRequest.Result as OkObjectResult;
        var dto = result!.Value as UserDto;

        Assert.NotNull(dto);
        Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
    }
}
