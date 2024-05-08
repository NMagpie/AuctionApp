using Application.App.Lots.Commands;
using Application.App.Lots.Responses;
using Application.Common.Abstractions;
using Application.Common.Exceptions;
using AuctionApp.Domain.Models;
using AutoMapper;
using Moq;
using System.Linq.Expressions;

namespace UnitTests.Application.Lots.Commands;
public class UpdateLotCommandTests
{
    [Fact]
    public async void UpdateLotGood()
    {
        var lotCommand = new UpdateLotCommand
        {
            Id = 1,
            Title = "Test1",
            Description = "Test",
            InitialPrice = 2,
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
            Title = "OldTitle",
            Description = "OldDescription",
            AuctionId = auction.Id,
            Auction = auction,
            InitialPrice = 1,
        };

        auction.Lots.Add(lot);

        var lotDto = new LotDto
        {
            Id = lot.Id,
            Title = lot.Title,
            Description = lot.Description,
            AuctionId = lot.AuctionId,
            InitialPrice = lot.InitialPrice,
        };

        var repositoryMock = new Mock<IRepository>();

        var mapperMock = new Mock<IMapper>();

        repositoryMock
            .Setup(x => x.GetByIdWithInclude<Lot>(
                It.IsAny<int>(),
                It.IsAny<Expression<Func<Lot, object>>[]>()
            ))
            .Returns(Task.FromResult<Lot?>(lot));

        mapperMock
            .Setup(x => x.Map(It.IsAny<UpdateLotCommand>(), It.IsAny<Lot>()))
            .Returns(lot);

        mapperMock
            .Setup(x => x.Map<Lot, LotDto>(It.IsAny<Lot>()))
            .Returns(lotDto);

        var updateLotHandler = new UpdateLotCommandHandler(repositoryMock.Object, mapperMock.Object);

        var result = await updateLotHandler.Handle(lotCommand, new CancellationToken());

        repositoryMock.Verify(x => x.GetByIdWithInclude<Lot>(
                It.IsAny<int>(),
                It.IsAny<Expression<Func<Lot, object>>[]>()
            ), Times.Once);

        mapperMock.Verify(x => x.Map(It.IsAny<UpdateLotCommand>(), It.IsAny<Lot>()), Times.Once);

        repositoryMock.Verify(x => x.SaveChanges(), Times.Once);

        mapperMock.Verify(x => x.Map<Lot, LotDto>(It.IsAny<Lot>()), Times.Once);
    }

    [Fact]
    public async void UpdateLotNotFound()
    {
        var lotCommand = new UpdateLotCommand
        {
            Id = 1,
            Title = "Test1",
            Description = "Test",
            InitialPrice = 2,
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
            Title = "OldTitle",
            Description = "OldDescription",
            AuctionId = auction.Id,
            Auction = auction,
            InitialPrice = 1,
        };

        auction.Lots.Add(lot);

        var repositoryMock = new Mock<IRepository>();

        var mapperMock = new Mock<IMapper>();

        repositoryMock
            .Setup(x => x.GetByIdWithInclude<Lot>(
                It.IsAny<int>(),
                It.IsAny<Expression<Func<Lot, object>>[]>()
            ))
            .Returns(Task.FromResult<Lot?>(lot));

        var updateLotHandler = new UpdateLotCommandHandler(repositoryMock.Object, mapperMock.Object);

        await Assert.ThrowsAsync<BusinessValidationException>(async () => await updateLotHandler.Handle(lotCommand, new CancellationToken()));

        repositoryMock.Verify(x => x.GetByIdWithInclude<Lot>(
                It.IsAny<int>(),
                It.IsAny<Expression<Func<Lot, object>>[]>()
            ), Times.Once);

        mapperMock.Verify(x => x.Map(It.IsAny<UpdateLotCommand>(), It.IsAny<Lot>()), Times.Never);

        repositoryMock.Verify(x => x.SaveChanges(), Times.Never);

        mapperMock.Verify(x => x.Map<Lot, LotDto>(It.IsAny<Lot>()), Times.Never);
    }

    [Fact]
    public async void UpdateLotInLockedTime()
    {
        var lotCommand = new UpdateLotCommand
        {
            Id = 1,
            Title = "Test1",
            Description = "Test",
            InitialPrice = 2,
        };

        var repositoryMock = new Mock<IRepository>();

        var mapperMock = new Mock<IMapper>();

        repositoryMock
            .Setup(x => x.GetByIdWithInclude<Lot>(
                It.IsAny<int>(),
                It.IsAny<Expression<Func<Lot, object>>[]>()
            ))
            .Returns(Task.FromResult<Lot?>(null));

        var updateLotHandler = new UpdateLotCommandHandler(repositoryMock.Object, mapperMock.Object);

        await Assert.ThrowsAsync<EntityNotFoundException>(async () => await updateLotHandler.Handle(lotCommand, new CancellationToken()));

        repositoryMock.Verify(x => x.GetByIdWithInclude<Lot>(
                It.IsAny<int>(),
                It.IsAny<Expression<Func<Lot, object>>[]>()
            ), Times.Once);

        mapperMock.Verify(x => x.Map(It.IsAny<UpdateLotCommand>(), It.IsAny<Lot>()), Times.Never);

        repositoryMock.Verify(x => x.SaveChanges(), Times.Never);

        mapperMock.Verify(x => x.Map<Lot, LotDto>(It.IsAny<Lot>()), Times.Never);
    }
}
