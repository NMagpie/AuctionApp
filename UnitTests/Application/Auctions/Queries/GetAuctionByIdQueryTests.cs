using Application.App.Auctions.Queries;
using Application.App.Auctions.Responses;
using Application.Common.Abstractions;
using Application.Common.Exceptions;
using AuctionApp.Domain.Models;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;

namespace UnitTests.Application.Auctions.Queries;
public class GetAuctionByIdQueryTests
{
    [Fact]
    public async void GetAuctionGood()
    {
        var auctionQuery = new GetAuctionByIdQuery
        {
            Id = 1,
        };

        var auction = new Auction
        {
            Id = 1,
            Title = "A",
            CreatorId = 1,
            StartTime = DateTimeOffset.UtcNow,
            EndTime = DateTimeOffset.UtcNow,
            Lots = [],
        };

        var auctionDto = new AuctionDto
        {
            Id = auction.Id,
            Title = auction.Title,
            CreatorId = auction.CreatorId,
            StartTime = auction.StartTime,
            EndTime = auction.EndTime,
            LotIds = [],
        };

        var repositoryMock = new Mock<IRepository>();

        var mapperMock = new Mock<IMapper>();

        var loggerMock = new Mock<ILogger<GetAuctionByIdQueryHandler>>();

        repositoryMock
            .Setup(x => x.GetById<Auction>(It.IsAny<int>()))
            .Returns(Task.FromResult<Auction?>(auction));

        mapperMock
            .Setup(x => x.Map<Auction, AuctionDto>(It.IsAny<Auction>()))
            .Returns(auctionDto);

        var getAuctionByIdQueryHandler = new GetAuctionByIdQueryHandler(repositoryMock.Object, loggerMock.Object, mapperMock.Object);

        await getAuctionByIdQueryHandler.Handle(auctionQuery, new CancellationToken());

        repositoryMock.Verify(x => x.GetById<Auction>(It.IsAny<int>()), Times.Once);

        mapperMock.Verify(x => x.Map<Auction, AuctionDto>(It.IsAny<Auction>()), Times.Once);
    }

    [Fact]
    public async void GetAuctionNotFound()
    {
        var auctionQuery = new GetAuctionByIdQuery
        {
            Id = 1,
        };

        var repositoryMock = new Mock<IRepository>();

        var mapperMock = new Mock<IMapper>();

        var loggerMock = new Mock<ILogger<GetAuctionByIdQueryHandler>>();

        repositoryMock
            .Setup(x => x.GetById<Auction>(It.IsAny<int>()))
            .Returns(Task.FromResult<Auction?>(null));

        var getAuctionByIdQueryHandler = new GetAuctionByIdQueryHandler(repositoryMock.Object, loggerMock.Object, mapperMock.Object);

        await Assert.ThrowsAsync<EntityNotFoundException>(async () => await getAuctionByIdQueryHandler.Handle(auctionQuery, new CancellationToken()));

        repositoryMock.Verify(x => x.GetById<Auction>(It.IsAny<int>()), Times.Once);

        mapperMock.Verify(x => x.Map<Auction, AuctionDto>(It.IsAny<Auction>()), Times.Never);
    }
}
