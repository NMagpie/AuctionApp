using Application.App.Bids.Commands;
using Application.App.Bids.Responses;
using Application.Common.Abstractions;
using Application.Common.Exceptions;
using AuctionApp.Domain.Models;
using AutoMapper;
using Moq;
using System.Linq.Expressions;

namespace UnitTests.Application.Bids.Commands;
public class CreateBidCommandTests
{
    [Fact]
    public async void CreateBidGood()
    {
        var bidCommand = new CreateBidCommand
        {
            LotId = 1,
            UserId = 1,
            Amount = 2,
        };

        var auction = new Auction
        {
            Id = 1,
            Title = "A",
            CreatorId = 1,
            StartTime = DateTimeOffset.UtcNow,
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
            LotId = bidCommand.LotId,
            UserId = bidCommand.UserId,
            Amount = bidCommand.Amount,
        };

        var bidDto = new BidDto
        {
            Id = bidCommand.LotId,
            LotId = lot.Id,
            UserId = bidCommand.UserId,
            Amount = bidCommand.Amount,
            CreateTime = DateTimeOffset.UtcNow,
            IsWon = false,
        };

        var repositoryMock = new Mock<IEntityRepository>();

        var mapperMock = new Mock<IMapper>();

        repositoryMock
            .Setup(x => x.GetByIdWithInclude<Lot>(It.IsAny<int>(), It.IsAny<Expression<Func<Lot, object>>>()))
            .Returns(Task.FromResult<Lot?>(lot));

        repositoryMock
            .Setup(x => x.Add(It.IsAny<Bid>()))
            .Returns(Task.FromResult(bid));

        mapperMock
            .Setup(x => x.Map<CreateBidCommand, Bid>(It.IsAny<CreateBidCommand>()))
            .Returns(bid);

        mapperMock
            .Setup(x => x.Map<Bid, BidDto>(It.IsAny<Bid>()))
            .Returns(bidDto);

        var createBidHandler = new CreateBidCommandHandler(repositoryMock.Object, mapperMock.Object);

        var result = await createBidHandler.Handle(bidCommand, new CancellationToken());

        repositoryMock.Verify(x => x.GetByIdWithInclude<Lot>(It.IsAny<int>(), It.IsAny<Expression<Func<Lot, object>>>()), Times.Once);

        mapperMock.Verify(x => x.Map<CreateBidCommand, Bid>(It.IsAny<CreateBidCommand>()), Times.Once);

        repositoryMock.Verify(x => x.Add(It.IsAny<Bid>()), Times.Once);

        repositoryMock.Verify(x => x.SaveChanges(), Times.Once);

        mapperMock.Verify(x => x.Map<Bid, BidDto>(It.IsAny<Bid>()), Times.Once);
    }

    [Fact]
    public async void CreateBidAuctionFinished()
    {
        var bidCommand = new CreateBidCommand
        {
            LotId = 1,
            UserId = 1,
            Amount = 2,
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
        };

        var repositoryMock = new Mock<IEntityRepository>();

        var mapperMock = new Mock<IMapper>();

        repositoryMock
            .Setup(x => x.GetByIdWithInclude<Lot>(It.IsAny<int>(), It.IsAny<Expression<Func<Lot, object>>>()))
            .Returns(Task.FromResult<Lot?>(lot));

        var createBidHandler = new CreateBidCommandHandler(repositoryMock.Object, mapperMock.Object);

        await Assert.ThrowsAsync<BusinessValidationException>( async () => await createBidHandler.Handle(bidCommand, new CancellationToken()));

        repositoryMock.Verify(x => x.GetByIdWithInclude<Lot>(It.IsAny<int>(), It.IsAny<Expression<Func<Lot, object>>>()), Times.Once);

        mapperMock.Verify(x => x.Map<CreateBidCommand, Bid>(It.IsAny<CreateBidCommand>()), Times.Never);

        repositoryMock.Verify(x => x.Add(It.IsAny<Bid>()), Times.Never);

        repositoryMock.Verify(x => x.SaveChanges(), Times.Never);

        mapperMock.Verify(x => x.Map<Bid, BidDto>(It.IsAny<Bid>()), Times.Never);
    }


    [Fact]
    public async void CreateBidLotNotFound()
    {
        var bidCommand = new CreateBidCommand
        {
            LotId = 1,
            UserId = 1,
            Amount = 2,
        };

        var repositoryMock = new Mock<IEntityRepository>();

        var mapperMock = new Mock<IMapper>();

        repositoryMock
            .Setup(x => x.GetByIdWithInclude<Lot>(It.IsAny<int>(), It.IsAny<Expression<Func<Lot, object>>>()))
            .Returns(Task.FromResult<Lot?>(null));

        var createBidHandler = new CreateBidCommandHandler(repositoryMock.Object, mapperMock.Object);

        await Assert.ThrowsAsync<EntityNotFoundException>(async () => await createBidHandler.Handle(bidCommand, new CancellationToken()));

        repositoryMock.Verify(x => x.GetByIdWithInclude<Lot>(It.IsAny<int>(), It.IsAny<Expression<Func<Lot, object>>>()), Times.Once);

        mapperMock.Verify(x => x.Map<CreateBidCommand, Bid>(It.IsAny<CreateBidCommand>()), Times.Never);

        repositoryMock.Verify(x => x.Add(It.IsAny<Bid>()), Times.Never);

        repositoryMock.Verify(x => x.SaveChanges(), Times.Never);

        mapperMock.Verify(x => x.Map<Bid, BidDto>(It.IsAny<Bid>()), Times.Never);
    }
}
