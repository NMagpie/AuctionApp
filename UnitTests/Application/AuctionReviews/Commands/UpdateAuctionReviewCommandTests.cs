using Application.App.AuctionReviews.Commands;
using Application.App.AuctionReviews.Responses;
using Application.Common.Abstractions;
using Application.Common.Exceptions;
using AuctionApp.Domain.Models;
using AutoMapper;
using Moq;

namespace UnitTests.Application.AuctionReviews.Commands;
public class UpdateAuctionReviewCommandTests
{
    [Fact]
    public async void UpdateAuctionReviewGood()
    {
        var auctionReview = new AuctionReview
        {
            UserId = 1,
            AuctionId = 1,
            ReviewText = "122",
            Rating = 2,
        };

        var updatedAuctionReview = new UpdateAuctionReviewCommand
        {
            Id = 1,
            ReviewText = "123",
            Rating = 1,
        };

        var updatedAuctionReviewDto = new AuctionReviewDto
        {
            Id = 1,
            UserId = auctionReview.UserId,
            AuctionId = auctionReview.AuctionId,
            ReviewText = updatedAuctionReview.ReviewText,
            Rating = updatedAuctionReview.Rating,
        };

        var repositoryMock = new Mock<IEntityRepository>();

        var mapperMock = new Mock<IMapper>();

        repositoryMock
            .Setup(x => x.GetById<AuctionReview>(It.IsAny<int>()))
            .Returns(Task.FromResult<AuctionReview?>(auctionReview));

        mapperMock
            .Setup(x => x.Map<AuctionReview, AuctionReviewDto>(It.IsAny<AuctionReview>()))
            .Returns(updatedAuctionReviewDto);

        var updateAuctionReviewHandler = new UpdateAuctionReviewCommandHandler(repositoryMock.Object, mapperMock.Object);

        var result = await updateAuctionReviewHandler.Handle(updatedAuctionReview, new CancellationToken());

        repositoryMock.Verify(x => x.GetById<AuctionReview>(It.IsAny<int>()), Times.Once);

        mapperMock.Verify(x => x.Map<AuctionReview, AuctionReviewDto>(It.IsAny<AuctionReview>()), Times.Once);

        repositoryMock.Verify(x => x.SaveChanges(), Times.Once);

        Assert.Equal(auctionReview.UserId, result.UserId);
        Assert.Equal(auctionReview.AuctionId, result.AuctionId);
        Assert.Equal(updatedAuctionReview.ReviewText, result.ReviewText);
        Assert.Equal(updatedAuctionReview.Rating, result.Rating);
    }

    [Fact]
    public async void UpdateAuctionReviewNotFound()
    {
        var updatedAuctionReview = new UpdateAuctionReviewCommand
        {
            Id = 1,
            ReviewText = "123",
            Rating = 1,
        };

        var repositoryMock = new Mock<IEntityRepository>();

        var mapperMock = new Mock<IMapper>();

        repositoryMock
            .Setup(x => x.GetById<AuctionReview>(It.IsAny<int>()))
            .Returns(Task.FromResult<AuctionReview?>(null));

        var updateAuctionReviewHandler = new UpdateAuctionReviewCommandHandler(repositoryMock.Object, mapperMock.Object);

        await Assert.ThrowsAsync<EntityNotFoundException>(async () => await updateAuctionReviewHandler.Handle(updatedAuctionReview, new CancellationToken()));

        repositoryMock.Verify(x => x.GetById<AuctionReview>(It.IsAny<int>()), Times.Once);

        mapperMock.Verify(x => x.Map<AuctionReview, AuctionReviewDto>(It.IsAny<AuctionReview>()), Times.Never);
    }
}