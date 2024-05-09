using Application.App.AuctionReviews.Commands;
using Application.App.AuctionReviews.Responses;
using Infrastructure.Persistance.Repositories;
using IntegrationTests.Helpers;
using Microsoft.AspNetCore.Mvc;
using Presentation.Controllers;
using System.Net;

namespace IntegrationTests.Presentation.Controllers;
public class AuctionReviewsControllerTests
{
    [Fact]
    public async void AuctionReviewsController_GetAuctionReviewGood()
    {
        var nAuctions = 2;
        var nUsers = 1;
        var nReviews = 1;

        using var contextBuilder = new DataContextBuilder();

        contextBuilder.SeedUsers(nUsers);
        contextBuilder.SeedAuctions(nAuctions);
        contextBuilder.SeedAuctionReviews(nReviews);

        var dbContext = contextBuilder.GetContext();

        var entityRepository = new EntityRepository(dbContext);

        var mediator = TestHelpers.CreateMediator(entityRepository);

        var controller = new AuctionReviewsController(mediator);

        var reviewId = 1;

        var resultRequest = await controller.GetAuctionReview(reviewId);

        var result = resultRequest.Result as OkObjectResult;
        var dto = result!.Value as AuctionReviewDto;

        Assert.NotNull(dto);
        Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
    }

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

        var mediator = TestHelpers.CreateMediator(entityRepository);

        var controller = new AuctionReviewsController(mediator);

        var command = new CreateAuctionReviewCommand
        {
            UserId = 1,
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

    [Fact]
    public async void AuctionReviewsController_PutAuctionReviewGood()
    {
        var nAuctions = 2;
        var nUsers = 1;
        var nReviews = 1;

        using var contextBuilder = new DataContextBuilder();

        contextBuilder.SeedUsers(nUsers);
        contextBuilder.SeedAuctions(nAuctions);
        contextBuilder.SeedAuctionReviews(nReviews);

        var dbContext = contextBuilder.GetContext();

        var entityRepository = new EntityRepository(dbContext);

        var mediator = TestHelpers.CreateMediator(entityRepository);

        var controller = new AuctionReviewsController(mediator);

        var command = new UpdateAuctionReviewCommand
        {
            Id = 1,
            ReviewText = "Test",
            Rating = 4,
        };

        var resultRequest = await controller.UpdateAuctionReview(command);

        var result = resultRequest.Result as OkObjectResult;
        var dto = result!.Value as AuctionReviewDto;

        Assert.NotNull(dto);
        Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
    }

    [Fact]
    public async void AuctionReviewsController_DeleteAuctionReviewGood()
    {
        var nAuctions = 2;
        var nUsers = 1;
        var nReviews = 1;

        using var contextBuilder = new DataContextBuilder();

        contextBuilder.SeedUsers(nUsers);
        contextBuilder.SeedAuctions(nAuctions);
        contextBuilder.SeedAuctionReviews(nReviews);

        var dbContext = contextBuilder.GetContext();

        var entityRepository = new EntityRepository(dbContext);

        var mediator = TestHelpers.CreateMediator(entityRepository);

        var controller = new AuctionReviewsController(mediator);

        var reviewId = 1;

        var resultRequest = await controller.DeleteAuctionReview(reviewId);

        var result = resultRequest as OkResult;

        Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
    }
}
