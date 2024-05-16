using Application.App.Bids.Responses;
using Application.App.Queries;
using Application.Common.Abstractions;
using Application.Common.Exceptions;
using AuctionApp.Domain.Models;
using AutoMapper;
using Moq;

namespace UnitTests.Application.Bids.Queries;
public class GetBidByIdQueryTests
{
    [Fact]
    public async void GetBidGood()
    {
        var bidCommand = new GetBidByIdQuery
        {
            Id = 1,
        };

        var bid = new Bid
        {
            Id = 1,
            LotId = 1,
            UserId = 1,
            Amount = 2,
            CreateTime = DateTimeOffset.UtcNow,
        };

        var bidDto = new BidDto
        {
            Id = bid.Id,
            LotId = bid.LotId,
            UserId = bid.UserId,
            Amount = bid.Amount,
            CreateTime = DateTimeOffset.UtcNow,
            IsWon = false,
        };

        var repositoryMock = new Mock<IEntityRepository>();

        var mapperMock = new Mock<IMapper>();

        repositoryMock
            .Setup(x => x.GetById<Bid>(It.IsAny<int>()))
            .Returns(Task.FromResult<Bid?>(bid));

        mapperMock
            .Setup(x => x.Map<Bid, BidDto>(It.IsAny<Bid>()))
            .Returns(bidDto);

        var getBidHandler = new GetBidByIdQueryHandler(repositoryMock.Object, mapperMock.Object);

        var result = await getBidHandler.Handle(bidCommand, new CancellationToken());

        repositoryMock.Verify(x => x.GetById<Bid>(It.IsAny<int>()), Times.Once);

        mapperMock.Verify(x => x.Map<Bid, BidDto>(It.IsAny<Bid>()), Times.Once);
    }

    [Fact]
    public async void GetBidNotFound()
    {
        var bidCommand = new GetBidByIdQuery
        {
            Id = 1,
        };

        var repositoryMock = new Mock<IEntityRepository>();

        var mapperMock = new Mock<IMapper>();

        repositoryMock
            .Setup(x => x.GetById<Bid>(It.IsAny<int>()))
            .Returns(Task.FromResult<Bid?>(null));

        var getBidHandler = new GetBidByIdQueryHandler(repositoryMock.Object, mapperMock.Object);

        await Assert.ThrowsAsync<EntityNotFoundException>(async () => await getBidHandler.Handle(bidCommand, new CancellationToken()));

        repositoryMock.Verify(x => x.GetById<Bid>(It.IsAny<int>()), Times.Once);

        mapperMock.Verify(x => x.Map<Bid, BidDto>(It.IsAny<Bid>()), Times.Never);
    }
}

