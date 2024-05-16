using Application.App.AuctionReviews.Responses;
using Application.App.Queries;
using Application.Common.Abstractions;
using Application.Common.Exceptions;
using AuctionApp.Domain.Models;
using AutoMapper;
using Moq;

namespace UnitTests.Application.AuctionReviews.Queries;
public class GetAuctionReviewByIdQueryTests
{
    [Fact]
    public async void GetAuctionReviewGood()
    {
        var auctionReviewQuery = new GetAuctionReviewByIdQuery
        {
            Id = 1,
        };

        var auctionReview = new AuctionReview
        {
            Id = 1,
            UserId = 1,
            AuctionId = 1,
            ReviewText = "123",
            Rating = 1,
        };

        var auctionReviewDto = new AuctionReviewDto
        {
            Id = auctionReview.Id,
            UserId = auctionReview.UserId,
            AuctionId = auctionReview.AuctionId,
            ReviewText = auctionReview.ReviewText,
            Rating = auctionReview.Rating,
        };

        var repositoryMock = new Mock<IEntityRepository>();

        var mapperMock = new Mock<IMapper>();

        repositoryMock
            .Setup(x => x.GetById<AuctionReview>(It.IsAny<int>()))
            .Returns(Task.FromResult<AuctionReview?>(auctionReview));

        mapperMock
            .Setup(x => x.Map<AuctionReview, AuctionReviewDto>(It.IsAny<AuctionReview>()))
            .Returns(auctionReviewDto);

        var getAuctionReviewByIdQueryHandler = new GetAuctionReviewByIdQueryHandler(repositoryMock.Object, mapperMock.Object);

        await getAuctionReviewByIdQueryHandler.Handle(auctionReviewQuery, new CancellationToken());

        repositoryMock.Verify(x => x.GetById<AuctionReview>(It.IsAny<int>()), Times.Once);

        mapperMock.Verify(x => x.Map<AuctionReview, AuctionReviewDto>(It.IsAny<AuctionReview>()), Times.Once);
    }

    [Fact]
    public async void GetAuctionReviewNotFound()
    {
        var auctionReviewQuery = new GetAuctionReviewByIdQuery
        {
            Id = 1,
        };

        var repositoryMock = new Mock<IEntityRepository>();

        var mapperMock = new Mock<IMapper>();

        repositoryMock
            .Setup(x => x.GetById<AuctionReview>(It.IsAny<int>()))
            .Returns(Task.FromResult<AuctionReview?>(null));

        var getAuctionReviewByIdQueryHandler = new GetAuctionReviewByIdQueryHandler(repositoryMock.Object, mapperMock.Object);

        await Assert.ThrowsAsync<EntityNotFoundException>(async () => await getAuctionReviewByIdQueryHandler.Handle(auctionReviewQuery, new CancellationToken()));

        repositoryMock.Verify(x => x.GetById<AuctionReview>(It.IsAny<int>()), Times.Once);

        mapperMock.Verify(x => x.Map<AuctionReview, AuctionReviewDto>(It.IsAny<AuctionReview>()), Times.Never);
    }
}
