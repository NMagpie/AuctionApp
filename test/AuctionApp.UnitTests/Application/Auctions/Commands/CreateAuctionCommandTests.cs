using Application.App.Auctions.Commands;
using Application.App.Auctions.Responses;
using Application.Common.Abstractions;
using Application.Common.Exceptions;
using Application.Common.Models;
using AuctionApp.Domain.Models;
using AutoMapper;
using Domain.Auth;
using Microsoft.Extensions.Logging;
using Moq;

namespace UnitTests.Application.Auctions.Commands;
public class CreateAuctionCommandTests
{
    [Fact]
    public async void CreateAuctionGood()
    {
        var auctionCommand = new CreateAuctionCommand
        {
            Title = "A",
            CreatorId = 1,
            StartTime = DateTimeOffset.UtcNow + TimeSpan.FromDays(1),
            EndTime = DateTimeOffset.UtcNow + TimeSpan.FromDays(2),
            Lots = [
                new LotInAuctionDto {
                    Title = "Lot1",
                    Description = "Lot1Description",
                    InitialPrice = 1
                }
                ],
        };

        var auction = new Auction
        {
            Id = 1,
            Title = auctionCommand.Title,
            CreatorId = auctionCommand.CreatorId,
            StartTime = auctionCommand.StartTime,
            EndTime = auctionCommand.EndTime,
            Lots = [
                new Lot {
                    Title = auctionCommand.Lots[0].Title,
                    Description = auctionCommand.Lots[0].Description,
                    AuctionId = 1,
                    InitialPrice = auctionCommand.Lots[0].InitialPrice,
                }
                ]
        };

        var user = new User
        {
            Id = auctionCommand.CreatorId,
        };

        var repositoryMock = new Mock<IEntityRepository>();

        var userRepositoryMock = new Mock<IUserRepository>();

        var loggerMock = new Mock<ILogger<CreateAuctionCommandHandler>>();

        var mapperMock = new Mock<IMapper>();

        userRepositoryMock
            .Setup(x => x.GetById(It.IsAny<int>()))
            .Returns(Task.FromResult<User?>(user));

        repositoryMock
            .Setup(x => x.Add(It.IsAny<Auction>()))
            .Returns(Task.FromResult(auction));

        mapperMock
            .Setup(x => x.Map<CreateAuctionCommand, Auction>(It.IsAny<CreateAuctionCommand>()))
            .Returns(auction);

        var createAuctionHandler = new CreateAuctionCommandHandler(repositoryMock.Object, userRepositoryMock.Object, loggerMock.Object, mapperMock.Object);

        var result = await createAuctionHandler.Handle(auctionCommand, new CancellationToken());

        userRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Once);

        mapperMock.Verify(x => x.Map<CreateAuctionCommand, Auction>(It.IsAny<CreateAuctionCommand>()), Times.Once);

        repositoryMock.Verify(x => x.Add(It.IsAny<Auction>()), Times.Once);

        repositoryMock.Verify(x => x.SaveChanges(), Times.Once);

        mapperMock.Verify(x => x.Map<Auction, AuctionDto>(It.IsAny<Auction>()), Times.Once);
    }

    [Fact]
    public async void CreateAuctionUserNotFound()
    {
        var auctionCommand = new CreateAuctionCommand
        {
            Title = "A",
            CreatorId = 1,
            StartTime = DateTimeOffset.UtcNow + TimeSpan.FromDays(1),
            EndTime = DateTimeOffset.UtcNow + TimeSpan.FromDays(2),
            Lots = [
                new LotInAuctionDto {
                    Title = "Lot1",
                    Description = "Lot1Description",
                    InitialPrice = 1
                }
                ],
        };

        var auction = new Auction
        {
            Id = 1,
            Title = auctionCommand.Title,
            CreatorId = auctionCommand.CreatorId,
            StartTime = auctionCommand.StartTime,
            EndTime = auctionCommand.EndTime,
            Lots = [
                new Lot {
                    Title = auctionCommand.Lots[0].Title,
                    Description = auctionCommand.Lots[0].Description,
                    AuctionId = 1,
                    InitialPrice = auctionCommand.Lots[0].InitialPrice,
                }
                ]
        };

        var repositoryMock = new Mock<IEntityRepository>();

        var userRepositoryMock = new Mock<IUserRepository>();

        var loggerMock = new Mock<ILogger<CreateAuctionCommandHandler>>();

        var mapperMock = new Mock<IMapper>();

        userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(Task.FromResult<User?>(null));

        repositoryMock.Setup(x => x.Add(It.IsAny<Auction>())).Returns(Task.FromResult(auction));

        var createAuctionHandler = new CreateAuctionCommandHandler(repositoryMock.Object, userRepositoryMock.Object, loggerMock.Object, mapperMock.Object);

        await Assert.ThrowsAsync<EntityNotFoundException>(async () => await createAuctionHandler.Handle(auctionCommand, new CancellationToken()));

        userRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Once);

        mapperMock.Verify(x => x.Map<CreateAuctionCommand, Auction>(It.IsAny<CreateAuctionCommand>()), Times.Never);

        repositoryMock.Verify(x => x.Add(It.IsAny<Auction>()), Times.Never);

        mapperMock.Verify(x => x.Map<Auction, AuctionDto>(It.IsAny<Auction>()), Times.Never);
    }
}