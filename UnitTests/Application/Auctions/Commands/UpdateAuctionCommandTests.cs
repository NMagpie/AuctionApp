using Application.App.Auctions.Commands;
using Application.App.Auctions.Responses;
using Application.Common.Abstractions;
using Application.Common.Exceptions;
using AuctionApp.Domain.Models;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;

namespace UnitTests.Application.Auctions.Commands;
public class UpdateAuctionCommandTests
{
    [Fact]
    public async void UpdateAuctionGood()
    {
        var auction = new Auction
        {
            Id = 1,
            Title = "A",
            CreatorId = 1,
            StartTime = DateTimeOffset.UtcNow + TimeSpan.FromDays(1),
            EndTime = DateTimeOffset.UtcNow + TimeSpan.FromDays(2),
            Lots = [
                new Lot {
                    Title = "Lot1",
                    Description = "Lot1Description",
                    AuctionId = 1,
                    InitialPrice = 1,
                }
                ]
        };

        var updatedAuction = new UpdateAuctionCommand
        {
            Id = 1,
            Title = "123",
            StartTime = DateTimeOffset.UtcNow + TimeSpan.FromDays(2),
            EndTime = DateTimeOffset.UtcNow + TimeSpan.FromDays(3)
        };

        var updatedAuctionDto = new AuctionDto
        {
            Id = 1,
            Title = updatedAuction.Title,
            CreatorId = auction.CreatorId,
            StartTime = updatedAuction.StartTime,
            EndTime = updatedAuction.EndTime,
            LotIds = auction.Lots.Select(lot => lot.Id).ToHashSet(),
        };

        var repositoryMock = new Mock<IRepository>();

        var mapperMock = new Mock<IMapper>();

        var loggerMock = new Mock<ILogger<UpdateAuctionCommandHandler>>();

        repositoryMock
            .Setup(x => x.GetById<Auction>(It.IsAny<int>()))
            .Returns(Task.FromResult<Auction?>(auction));

        mapperMock
            .Setup(x => x.Map<Auction, AuctionDto>(It.IsAny<Auction>()))
            .Returns(updatedAuctionDto);

        var updateAuctionHandler = new UpdateAuctionCommandHandler(repositoryMock.Object, loggerMock.Object, mapperMock.Object);

        var result = await updateAuctionHandler.Handle(updatedAuction, new CancellationToken());

        repositoryMock.Verify(x => x.GetById<Auction>(It.IsAny<int>()), Times.Once);

        mapperMock.Verify(x => x.Map(It.IsAny<UpdateAuctionCommand>(), It.IsAny<Auction>()), Times.Once);

        repositoryMock.Verify(x => x.SaveChanges(), Times.Once);

        mapperMock.Verify(x => x.Map<Auction, AuctionDto>(It.IsAny<Auction>()), Times.Once);

        Assert.Equal(updatedAuction.Title, result.Title);
        Assert.Equal(updatedAuction.StartTime, result.StartTime);
        Assert.Equal(updatedAuction.EndTime, result.EndTime);
    }

    [Fact]
    public async void UpdateAuctionNotFound()
    {
        var updatedAuction = new UpdateAuctionCommand
        {
            Id = 1,
            Title = "123",
            StartTime = DateTimeOffset.UtcNow + TimeSpan.FromDays(2),
            EndTime = DateTimeOffset.UtcNow + TimeSpan.FromDays(3)
        };

        var repositoryMock = new Mock<IRepository>();

        var mapperMock = new Mock<IMapper>();

        var loggerMock = new Mock<ILogger<UpdateAuctionCommandHandler>>();

        repositoryMock
            .Setup(x => x.GetById<Auction>(It.IsAny<int>()))
            .Returns(Task.FromResult<Auction?>(null));

        var updateAuctionHandler = new UpdateAuctionCommandHandler(repositoryMock.Object, loggerMock.Object, mapperMock.Object);

        await Assert.ThrowsAsync<EntityNotFoundException>(async () => await updateAuctionHandler.Handle(updatedAuction, new CancellationToken()));

        repositoryMock.Verify(x => x.GetById<Auction>(It.IsAny<int>()), Times.Once);

        mapperMock.Verify(x => x.Map<UpdateAuctionCommand, Auction>(It.IsAny<UpdateAuctionCommand>()), Times.Never);

        repositoryMock.Verify(x => x.SaveChanges(), Times.Never);

        mapperMock.Verify(x => x.Map<Auction, AuctionDto>(It.IsAny<Auction>()), Times.Never);
    }

    [Fact]
    public async void UpdateAuctionInLockedTime()
    {
        var auction = new Auction
        {
            Id = 1,
            Title = "A",
            CreatorId = 1,
            StartTime = DateTimeOffset.UtcNow,
            EndTime = DateTimeOffset.UtcNow + TimeSpan.FromDays(1),
            Lots = [
                new Lot {
                    Title = "Lot1",
                    Description = "Lot1Description",
                    AuctionId = 1,
                    InitialPrice = 1,
                }
                ]
        };

        var updatedAuction = new UpdateAuctionCommand
        {
            Id = 1,
            Title = "123",
            StartTime = DateTimeOffset.UtcNow + TimeSpan.FromDays(1),
            EndTime = DateTimeOffset.UtcNow + TimeSpan.FromDays(2)
        };

        var repositoryMock = new Mock<IRepository>();

        var mapperMock = new Mock<IMapper>();

        var loggerMock = new Mock<ILogger<UpdateAuctionCommandHandler>>();

        repositoryMock
            .Setup(x => x.GetById<Auction>(It.IsAny<int>()))
            .Returns(Task.FromResult<Auction?>(auction));

        var updateAuctionHandler = new UpdateAuctionCommandHandler(repositoryMock.Object, loggerMock.Object, mapperMock.Object);

        await Assert.ThrowsAsync<BusinessValidationException>(async () => await updateAuctionHandler.Handle(updatedAuction, new CancellationToken()));

        repositoryMock.Verify(x => x.GetById<Auction>(It.IsAny<int>()), Times.Once);

        mapperMock.Verify(x => x.Map(It.IsAny<UpdateAuctionCommand>(), It.IsAny<Auction>()), Times.Never);

        repositoryMock.Verify(x => x.SaveChanges(), Times.Never);

        mapperMock.Verify(x => x.Map<Auction, AuctionDto>(It.IsAny<Auction>()), Times.Never);
    }
}