using Application.App.Lots.Responses;
using Application.App.Queries;
using Application.Common.Abstractions;
using Application.Common.Exceptions;
using AuctionApp.Domain.Models;
using AutoMapper;
using Moq;

namespace UnitTests.Application.Lots.Queries;
public class GetLotByIdQueryTests
{
    [Fact]
    public async void GetLotGood()
    {
        var lotCommand = new GetLotByIdQuery
        {
            Id = 1,
        };

        var lot = new Lot
        {
            Id = 1,
            Title = "Test1",
            Description = "Test",
            AuctionId = 1,
            InitialPrice = 1,
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
            .Setup(x => x.GetById<Lot>(It.IsAny<int>()))
            .Returns(Task.FromResult<Lot?>(lot));

        mapperMock
            .Setup(x => x.Map<Lot, LotDto>(It.IsAny<Lot>()))
            .Returns(lotDto);

        var getLotHandler = new GetLotByIdQueryHandler(repositoryMock.Object, mapperMock.Object);

        var result = await getLotHandler.Handle(lotCommand, new CancellationToken());

        repositoryMock.Verify(x => x.GetById<Lot>(It.IsAny<int>()), Times.Once);

        mapperMock.Verify(x => x.Map<Lot, LotDto>(It.IsAny<Lot>()), Times.Once);
    }

    [Fact]
    public async void GetLotNotFound()
    {
        var lotCommand = new GetLotByIdQuery
        {
            Id = 1,
        };

        var repositoryMock = new Mock<IEntityRepository>();

        var mapperMock = new Mock<IMapper>();

        repositoryMock
            .Setup(x => x.GetById<Lot>(It.IsAny<int>()))
            .Returns(Task.FromResult<Lot?>(null));

        var getLotHandler = new GetLotByIdQueryHandler(repositoryMock.Object, mapperMock.Object);

        await Assert.ThrowsAsync<EntityNotFoundException>(async () => await getLotHandler.Handle(lotCommand, new CancellationToken()));

        repositoryMock.Verify(x => x.GetById<Lot>(It.IsAny<int>()), Times.Once);

        mapperMock.Verify(x => x.Map<Lot, LotDto>(It.IsAny<Lot>()), Times.Never);
    }
}

