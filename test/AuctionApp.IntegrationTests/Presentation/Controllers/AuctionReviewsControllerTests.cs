using Application.App.AuctionReviews.Responses;
using AutoMapper;
using Infrastructure.Persistance.Repositories;
using IntegrationTests.Helpers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Presentation.Common.Requests.AuctionReviews;
using Presentation.Controllers;
using System.Net;

namespace IntegrationTests.Presentation.Controllers;
public class AuctionReviewsControllerTests
{

    [Fact]
    public async void AuctionReviewsController_PostAuctionReviewGood()
    {
        var nAuctions = 3;
        var nUsers = 3;

        using var contextBuilder = new DataContextBuilder();

        contextBuilder.SeedUsers(nUsers);
        contextBuilder.SeedAuctions(nAuctions);

        var dbContext = contextBuilder.GetContext();

        var entityRepository = new EntityRepository(dbContext);

        var servicesProvider = TestHelpers.GetServiceProvider(entityRepository);

        var mediator = servicesProvider.GetRequiredService<IMediator>();

        var mapper = servicesProvider.GetRequiredService<IMapper>();

        var controller = new AuctionReviewsController(mediator, mapper);

        var command = new CreateAuctionReviewRequest
        {
            AuctionId = 2,
            ReviewText = "Test",
            Rating = 4,
        };

        var resultRequest = await controller.CreateAuctionReview(command);

        var result = resultRequest.Result as OkObjectResult;
        var dto = result!.Value as AuctionReviewDto;

        Assert.NotNull(dto);
        Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
    }
}
