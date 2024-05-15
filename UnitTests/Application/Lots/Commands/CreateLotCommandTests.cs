using Application.App.Lots.Commands;
using Application.App.Lots.Responses;
using Application.Common.Abstractions;
using Application.Common.Exceptions;
using AuctionApp.Domain.Models;
using AutoMapper;
using Moq;

namespace UnitTests.Application.Lots.Commands;
public class CreateLotCommandTests
{
    [Fact]
    public async void CreateLotGood()
    {
        var lotCommand = new CreateLotCommand
        {
            Title = "Test1",
            Description = "Test",
            AuctionId = 1,
            InitialPrice = 1,
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
            Title = lotCommand.Title,
            Description = lotCommand.Description,
            AuctionId = lotCommand.AuctionId,
            Auction = auction,
            InitialPrice = lotCommand.InitialPrice,
        };

        var lotDto = new LotDto
        {
            Id = lot.Id,
            Title = lot.Title,
            Description = lot.Description,
            AuctionId = lot.AuctionId,
            InitialPrice = lot.InitialPrice,
        };

        var repositoryMock = new Mock<IEntityRepository>();

        var mapperMock = new Mock<IMapper>();

        repositoryMock
            .Setup(x => x.GetById<Auction>(It.IsAny<int>()))
            .Returns(Task.FromResult<Auction?>(auction));

        repositoryMock
            .Setup(x => x.Add(It.IsAny<Lot>()))
            .Returns(Task.FromResult(lot));

        mapperMock
            .Setup(x => x.Map<CreateLotCommand, Lot>(It.IsAny<CreateLotCommand>()))
            .Returns(lot);

        mapperMock
            .Setup(x => x.Map<Lot, LotDto>(It.IsAny<Lot>()))
            .Returns(lotDto);

        var createLotHandler = new CreateLotCommandHandler(repositoryMock.Object, mapperMock.Object);

        var result = await createLotHandler.Handle(lotCommand, new CancellationToken());

        repositoryMock.Verify(x => x.GetById<Auction>(It.IsAny<int>()), Times.Once);

        mapperMock.Verify(x => x.Map<CreateLotCommand, Lot>(It.IsAny<CreateLotCommand>()), Times.Once);

        repositoryMock.Verify(x => x.Add(It.IsAny<Lot>()), Times.Once);

        repositoryMock.Verify(x => x.SaveChanges(), Times.Once);

        mapperMock.Verify(x => x.Map<Lot, LotDto>(It.IsAny<Lot>()), Times.Once);
    }

    [Fact]
    public async void CreateLotAuctionNotFound()
    {
        var lotCommand = new CreateLotCommand
        {
            Title = "Test1",
            Description = "Test",
            AuctionId = 1,
            InitialPrice = 1,
        };

        var repositoryMock = new Mock<IEntityRepository>();

        var mapperMock = new Mock<IMapper>();

        repositoryMock
            .Setup(x => x.GetById<Auction>(It.IsAny<int>()))
            .Returns(Task.FromResult<Auction?>(null));

        var createLotHandler = new CreateLotCommandHandler(repositoryMock.Object, mapperMock.Object);

        await Assert.ThrowsAsync<EntityNotFoundException>(async () => await createLotHandler.Handle(lotCommand, new CancellationToken()));

        repositoryMock.Verify(x => x.GetById<Auction>(It.IsAny<int>()), Times.Once);

        mapperMock.Verify(x => x.Map<CreateLotCommand, Lot>(It.IsAny<CreateLotCommand>()), Times.Never);

        repositoryMock.Verify(x => x.Add(It.IsAny<Lot>()), Times.Never);

        repositoryMock.Verify(x => x.SaveChanges(), Times.Never);

        mapperMock.Verify(x => x.Map<Lot, LotDto>(It.IsAny<Lot>()), Times.Never);
    }

    [Fact]
    public async void CreateLotInLockedTime()
    {
        var lotCommand = new CreateLotCommand
        {
            Title = "Test1",
            Description = "Test",
            AuctionId = 1,
            InitialPrice = 1,
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
            Title = lotCommand.Title,
            Description = lotCommand.Description,
            AuctionId = lotCommand.AuctionId,
            Auction = auction,
            InitialPrice = lotCommand.InitialPrice,
        };

        var lotDto = new LotDto
        {
            Id = lot.Id,
            Title = lot.Title,
            Description = lot.Description,
            AuctionId = lot.AuctionId,
            InitialPrice = lot.InitialPrice,
        };

        var repositoryMock = new Mock<IEntityRepository>();

        var mapperMock = new Mock<IMapper>();

        repositoryMock
            .Setup(x => x.GetById<Auction>(It.IsAny<int>()))
            .Returns(Task.FromResult<Auction?>(auction));

        var createLotHandler = new CreateLotCommandHandler(repositoryMock.Object, mapperMock.Object);

        await Assert.ThrowsAsync<BusinessValidationException>(async () => await createLotHandler.Handle(lotCommand, new CancellationToken()));

        repositoryMock.Verify(x => x.GetById<Auction>(It.IsAny<int>()), Times.Once);

        mapperMock.Verify(x => x.Map<CreateLotCommand, Lot>(It.IsAny<CreateLotCommand>()), Times.Never);

        repositoryMock.Verify(x => x.Add(It.IsAny<Lot>()), Times.Never);

        repositoryMock.Verify(x => x.SaveChanges(), Times.Never);

        mapperMock.Verify(x => x.Map<Lot, LotDto>(It.IsAny<Lot>()), Times.Never);
    }
}
