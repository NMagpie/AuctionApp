using Application.App.Bids.Commands;
using Application.Common.Abstractions;
using Application.Common.Exceptions;
using AuctionApp.Domain.Models;
using Moq;
using System.Linq.Expressions;

namespace UnitTests.Application.Bids.Commands;
public class DeleteBidCommandTests
{
    [Fact]
    public async void DeleteBidGood()
    {
        var bidCommand = new DeleteBidCommand
        {
            Id = 1,
        };

        var auction = new Auction
        {
            Id = 1,
            Title = "A",
            CreatorId = 1,
            StartTime = DateTimeOffset.UtcNow - TimeSpan.FromSeconds(2),
            EndTime = DateTimeOffset.UtcNow + TimeSpan.FromDays(1),
            Lots = []
        };

        var lot = new Lot
        {
            Id = 1,
            Title = "Test1",
            Description = "Test",
            AuctionId = auction.Id,
            Auction = auction,
            InitialPrice = 1,
        };

        var bid = new Bid
        {
            Id = 1,
            LotId = 1,
            Lot = lot,
            UserId = 1,
            Amount = 2,
        };

        var repositoryMock = new Mock<IEntityRepository>();

        repositoryMock
            .Setup(x => x.GetByIdWithInclude<Bid>(
                It.IsAny<int>(),
                It.IsAny<Expression<Func<Bid, object>>[]>()
                ))
            .Returns(Task.FromResult<Bid?>(bid));

        var deleteBidHandler = new DeleteBidCommandHandler(repositoryMock.Object);

        await deleteBidHandler.Handle(bidCommand, new CancellationToken());

        repositoryMock.Verify(x => x.GetByIdWithInclude<Bid>(
            It.IsAny<int>(),
            It.IsAny<Expression<Func<Bid, object>>[]>()
            ), Times.Once);

        repositoryMock.Verify(x => x.Remove<Bid>(It.IsAny<int>()), Times.Once);

        repositoryMock.Verify(x => x.SaveChanges(), Times.Once);
    }

    [Fact]
    public async void DeleteBidNotFound()
    {
        var bidCommand = new DeleteBidCommand
        {
            Id = 1,
        };

        var repositoryMock = new Mock<IEntityRepository>();

        repositoryMock
            .Setup(x => x.GetByIdWithInclude<Bid>(
                It.IsAny<int>(),
                It.IsAny<Expression<Func<Bid, object>>[]>()
                ))
            .Returns(Task.FromResult<Bid?>(null));

        var deleteBidHandler = new DeleteBidCommandHandler(repositoryMock.Object);

        await Assert.ThrowsAsync<EntityNotFoundException>(async () => await deleteBidHandler.Handle(bidCommand, new CancellationToken()));

        repositoryMock.Verify(x => x.GetByIdWithInclude<Bid>(
            It.IsAny<int>(),
            It.IsAny<Expression<Func<Bid, object>>[]>()
            ), Times.Once);

        repositoryMock.Verify(x => x.Remove<Bid>(It.IsAny<int>()), Times.Never);

        repositoryMock.Verify(x => x.SaveChanges(), Times.Never);
    }

    [Fact]
    public async void DeleteBidAuctionFinished()
    {
        var bidCommand = new DeleteBidCommand
        {
            Id = 1,
        };

        var auction = new Auction
        {
            Id = 1,
            Title = "A",
            CreatorId = 1,
            StartTime = DateTimeOffset.UtcNow - TimeSpan.FromDays(1),
            EndTime = DateTimeOffset.UtcNow,
            Lots = []
        };

        var lot = new Lot
        {
            Id = 1,
            Title = "Test1",
            Description = "Test",
            AuctionId = auction.Id,
            Auction = auction,
            InitialPrice = 1,
        };

        var bid = new Bid
        {
            Id = 1,
            LotId = 1,
            Lot = lot,
            UserId = 1,
            Amount = 2,
        };

        var repositoryMock = new Mock<IEntityRepository>();

        repositoryMock
            .Setup(x => x.GetByIdWithInclude<Bid>(
                It.IsAny<int>(),
                It.IsAny<Expression<Func<Bid, object>>[]>()
                ))
            .Returns(Task.FromResult<Bid?>(bid));

        var deleteBidHandler = new DeleteBidCommandHandler(repositoryMock.Object);

        await Assert.ThrowsAsync<BusinessValidationException>(async () => await deleteBidHandler.Handle(bidCommand, new CancellationToken()));

        repositoryMock.Verify(x => x.GetByIdWithInclude<Bid>(
            It.IsAny<int>(),
            It.IsAny<Expression<Func<Bid, object>>[]>()
            ), Times.Once);

        repositoryMock.Verify(x => x.Remove<Bid>(It.IsAny<int>()), Times.Never);

        repositoryMock.Verify(x => x.SaveChanges(), Times.Never);
    }
}

