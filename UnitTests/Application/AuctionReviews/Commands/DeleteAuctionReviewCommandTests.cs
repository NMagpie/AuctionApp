using Application.App.AuctionReviews.Commands;
using Application.Common.Abstractions;
using AuctionApp.Domain.Models;
using Moq;

namespace UnitTests.Application.AuctionReviews.Commands;
public class DeleteAuctionReviewCommandTests
{
    [Fact]
    public async void DeleteAuctionReviewGood()
    {
        var auctionReviewCommand = new DeleteAuctionReviewCommand
        {
            Id = 1,
        };

        var repositoryMock = new Mock<IRepository>();

        repositoryMock.Setup(x => x.Remove<AuctionReview>(It.IsAny<int>())).Returns(Task.CompletedTask);

        var deleteAuctionReviewHandler = new DeleteAuctionReviewCommandHandler(repositoryMock.Object);

        await deleteAuctionReviewHandler.Handle(auctionReviewCommand, new CancellationToken());

        repositoryMock.Verify(x => x.Remove<AuctionReview>(It.IsAny<int>()), Times.Once);

        repositoryMock.Verify(x => x.SaveChanges(), Times.Once);
    }
}