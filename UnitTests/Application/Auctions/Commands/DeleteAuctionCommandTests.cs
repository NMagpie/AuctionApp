using Application.App.Auctions.Commands;
using Application.Common.Abstractions;
using Application.Common.Exceptions;
using AuctionApp.Domain.Models;
using Microsoft.Extensions.Logging;
using Moq;

namespace UnitTests.Application.Auctions.Commands;
public class DeleteAuctionCommandTests
{
    [Fact]
    public async void DeleteAuctionGood()
    {
        var auctionCommand = new DeleteAuctionCommand
        {
            Id = 1,
        };

        var auction = new Auction
        {
            StartTime = DateTimeOffset.UtcNow + TimeSpan.FromMinutes(10),
        };

        var repositoryMock = new Mock<IRepository>();

        var loggerMock = new Mock<ILogger<DeleteAuctionCommandHandler>>();

        repositoryMock
            .Setup(x => x.GetById<Auction>(It.IsAny<int>()))
            .Returns(Task.FromResult(auction));

        repositoryMock
            .Setup(x => x.Remove<Auction>(It.IsAny<int>()))
            .Returns(Task.CompletedTask);

        var deleteAuctionHandler = new DeleteAuctionCommandHandler(repositoryMock.Object, loggerMock.Object);

        await deleteAuctionHandler.Handle(auctionCommand, new CancellationToken());

        repositoryMock.Verify(x => x.Remove<Auction>(It.IsAny<int>()), Times.Once);

        repositoryMock.Verify(x => x.SaveChanges(), Times.Once);
    }

    [Fact]
    public async void DeleteAuctionNotFound()
    {
        var auctionCommand = new DeleteAuctionCommand
        {
            Id = 1,
        };

        var repositoryMock = new Mock<IRepository>();

        var loggerMock = new Mock<ILogger<DeleteAuctionCommandHandler>>();

        repositoryMock
            .Setup(x => x.GetById<Auction>(It.IsAny<int>()))
            .Returns(Task.FromResult<Auction?>(null));

        var deleteAuctionHandler = new DeleteAuctionCommandHandler(repositoryMock.Object, loggerMock.Object);

        await Assert.ThrowsAsync<EntityNotFoundException>( async () => await deleteAuctionHandler.Handle(auctionCommand, new CancellationToken()));

        repositoryMock.Verify(x => x.Remove<Auction>(It.IsAny<int>()), Times.Never);

        repositoryMock.Verify(x => x.SaveChanges(), Times.Never);
    }

    [Fact]
    public async void DeleteAuctionInLockedTime()
    {
        var auctionCommand = new DeleteAuctionCommand
        {
            Id = 1,
        };

        var auction = new Auction
        {
            StartTime = DateTimeOffset.UtcNow,
        };

        var repositoryMock = new Mock<IRepository>();

        var loggerMock = new Mock<ILogger<DeleteAuctionCommandHandler>>();

        repositoryMock
            .Setup(x => x.GetById<Auction>(It.IsAny<int>()))
            .Returns(Task.FromResult(auction));

        repositoryMock
            .Setup(x => x.Remove<Auction>(It.IsAny<int>()))
            .Returns(Task.CompletedTask);

        var deleteAuctionHandler = new DeleteAuctionCommandHandler(repositoryMock.Object, loggerMock.Object);

        await Assert.ThrowsAsync<BusinessValidationException>( async () => await deleteAuctionHandler.Handle(auctionCommand, new CancellationToken()));

        repositoryMock.Verify(x => x.Remove<Auction>(It.IsAny<int>()), Times.Never);

        repositoryMock.Verify(x => x.SaveChanges(), Times.Never);
    }
}