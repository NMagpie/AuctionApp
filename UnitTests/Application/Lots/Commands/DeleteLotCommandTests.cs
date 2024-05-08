using Application.App.Lots.Commands;
using Application.Common.Abstractions;
using Application.Common.Exceptions;
using AuctionApp.Domain.Models;
using Moq;
using System.Linq.Expressions;

namespace UnitTests.Application.Lots.Commands;
public class DeleteLotCommandTests
{
    [Fact]
    public async void DeleteLotGood()
    {
        var lotCommand = new DeleteLotCommand
        {
            Id = 1,
        };

        var auction = new Auction
        {
            Id = 1,
            Title = "A",
            CreatorId = 1,
            StartTime = DateTimeOffset.UtcNow + TimeSpan.FromMinutes(10),
            EndTime = DateTimeOffset.UtcNow + TimeSpan.FromDays(1),
            Lots = []
        };

        var lot = new Lot
        {
            Id = 1,
            Title = "Test1",
            Description = "Test",
            AuctionId = 1,
            Auction = auction,
            InitialPrice = 1,
        };

        auction.Lots.Add(lot);

        auction.Lots.Add(new Lot());

        var repositoryMock = new Mock<IRepository>();

        repositoryMock
            .Setup(x => x.GetByIdWithInclude<Lot>(
                It.IsAny<int>(),
                It.IsAny<Expression<Func<Lot, object>>[]>()
            ))
            .Returns(Task.FromResult<Lot?>(lot));

        var deleteLotHandler = new DeleteLotCommandHandler(repositoryMock.Object);

        await deleteLotHandler.Handle(lotCommand, new CancellationToken());

        repositoryMock.Verify(x => x.GetByIdWithInclude<Lot>(
                It.IsAny<int>(),
                It.IsAny<Expression<Func<Lot, object>>[]>()
            ), Times.Once);

        repositoryMock.Verify(x => x.Remove<Lot>(It.IsAny<int>()), Times.Once);

        repositoryMock.Verify(x => x.SaveChanges(), Times.Once);
    }

    [Fact]
    public async void DeleteLotNotFound()
    {
        var lotCommand = new DeleteLotCommand
        {
            Id = 1,
        };

        var repositoryMock = new Mock<IRepository>();

        repositoryMock
            .Setup(x => x.GetByIdWithInclude<Lot>(
                It.IsAny<int>(),
                It.IsAny<Expression<Func<Lot, object>>[]>()
            ))
            .Returns(Task.FromResult<Lot?>(null));

        var deleteLotHandler = new DeleteLotCommandHandler(repositoryMock.Object);

        await Assert.ThrowsAsync<EntityNotFoundException>(async () => await deleteLotHandler.Handle(lotCommand, new CancellationToken()));

        repositoryMock.Verify(x => x.GetByIdWithInclude<Lot>(
                It.IsAny<int>(),
                It.IsAny<Expression<Func<Lot, object>>[]>()
            ), Times.Once);

        repositoryMock.Verify(x => x.Remove<Lot>(It.IsAny<int>()), Times.Never);

        repositoryMock.Verify(x => x.SaveChanges(), Times.Never);
    }

    [Fact]
    public async void DeleteLotInLockedTime()
    {
        var lotCommand = new DeleteLotCommand
        {
            Id = 1,
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
            AuctionId = 1,
            Auction = auction,
            InitialPrice = 1,
        };

        auction.Lots.Add(lot);

        auction.Lots.Add(new Lot());

        var repositoryMock = new Mock<IRepository>();

        repositoryMock
            .Setup(x => x.GetByIdWithInclude<Lot>(
                It.IsAny<int>(),
                It.IsAny<Expression<Func<Lot, object>>[]>()
            ))
            .Returns(Task.FromResult<Lot?>(lot));

        var deleteLotHandler = new DeleteLotCommandHandler(repositoryMock.Object);

        await Assert.ThrowsAsync<BusinessValidationException>(async () => await deleteLotHandler.Handle(lotCommand, new CancellationToken()));

        repositoryMock.Verify(x => x.GetByIdWithInclude<Lot>(
                It.IsAny<int>(),
                It.IsAny<Expression<Func<Lot, object>>[]>()
            ), Times.Once);

        repositoryMock.Verify(x => x.Remove<Lot>(It.IsAny<int>()), Times.Never);

        repositoryMock.Verify(x => x.SaveChanges(), Times.Never);
    }

    [Fact]
    public async void DeleteLotOneLeftInAuction()
    {
        var lotCommand = new DeleteLotCommand
        {
            Id = 1,
        };

        var auction = new Auction
        {
            Id = 1,
            Title = "A",
            CreatorId = 1,
            StartTime = DateTimeOffset.UtcNow + TimeSpan.FromMinutes(10),
            EndTime = DateTimeOffset.UtcNow + TimeSpan.FromDays(1),
            Lots = []
        };

        var lot = new Lot
        {
            Id = 1,
            Title = "Test1",
            Description = "Test",
            AuctionId = 1,
            Auction = auction,
            InitialPrice = 1,
        };

        auction.Lots.Add(lot);

        var repositoryMock = new Mock<IRepository>();

        repositoryMock
            .Setup(x => x.GetByIdWithInclude<Lot>(
                It.IsAny<int>(),
                It.IsAny<Expression<Func<Lot, object>>[]>()
            ))
            .Returns(Task.FromResult<Lot?>(lot));

        var deleteLotHandler = new DeleteLotCommandHandler(repositoryMock.Object);

        await Assert.ThrowsAsync<BusinessValidationException>(async () => await deleteLotHandler.Handle(lotCommand, new CancellationToken()));

        repositoryMock.Verify(x => x.GetByIdWithInclude<Lot>(
                It.IsAny<int>(),
                It.IsAny<Expression<Func<Lot, object>>[]>()
            ), Times.Once);

        repositoryMock.Verify(x => x.Remove<Lot>(It.IsAny<int>()), Times.Never);

        repositoryMock.Verify(x => x.SaveChanges(), Times.Never);
    }
}

