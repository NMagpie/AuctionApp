using Application.App.UserWatchlists.Commands;
using Application.App.UserWatchlists.Responses;
using Application.Common.Abstractions;
using Application.Common.Exceptions;
using AuctionApp.Domain.Models;
using AutoMapper;
using Moq;

namespace UnitTests.Application.UserWatchlists.Commands;
public class CreateUserWatchlistCommandTests
{
    [Fact]
    public async void CreateUserWatchlistGood()
    {
        var userWatchlistCommand = new CreateUserWatchlistCommand
        {
            UserId = 1,
            AuctionId = 1,
        };

        var user = new User();

        var auction = new Auction();

        var userWatchlist = new UserWatchlist
        {
            Id = 1,
            UserId = 1,
            AuctionId = 1,
        };

        var userWatchlistDto = new UserWatchlistDto
        {
            Id = userWatchlist.Id,
            UserId = userWatchlist.UserId,
            AuctionId = userWatchlist.AuctionId,
        };

        var repositoryMock = new Mock<IRepository>();

        var mapperMock = new Mock<IMapper>();

        repositoryMock
            .Setup(x => x.GetById<User>(It.IsAny<int>()))
            .Returns(Task.FromResult(user));

        repositoryMock
            .Setup(x => x.GetById<Auction>(It.IsAny<int>()))
            .Returns(Task.FromResult(auction));

        repositoryMock
            .Setup(x => x.Add(It.IsAny<UserWatchlist>()))
            .Returns(Task.FromResult(userWatchlist));

        mapperMock
            .Setup(x => x.Map<CreateUserWatchlistCommand, UserWatchlist>(It.IsAny<CreateUserWatchlistCommand>()))
            .Returns(userWatchlist);

        mapperMock
            .Setup(x => x.Map<UserWatchlist, UserWatchlistDto>(It.IsAny<UserWatchlist>()))
            .Returns(userWatchlistDto);

        var createUserWatchlistCommandHandler = new CreateUserWatchlistCommandHandler(repositoryMock.Object, mapperMock.Object);

        var result = await createUserWatchlistCommandHandler.Handle(userWatchlistCommand, new CancellationToken());

        mapperMock.Verify(x => x.Map<CreateUserWatchlistCommand, UserWatchlist>(It.IsAny<CreateUserWatchlistCommand>()), Times.Once);

        repositoryMock.Verify(x => x.Add(It.IsAny<UserWatchlist>()), Times.Once);

        repositoryMock.Verify(x => x.SaveChanges(), Times.Once);

        mapperMock.Verify(x => x.Map<UserWatchlist, UserWatchlistDto>(It.IsAny<UserWatchlist>()), Times.Once);
    }

    [Fact]
    public async void CreateUserWatchlistUserNotFound()
    {
        var userWatchlistCommand = new CreateUserWatchlistCommand
        {
            UserId = 1,
            AuctionId = 1,
        };

        var repositoryMock = new Mock<IRepository>();

        var mapperMock = new Mock<IMapper>();

        repositoryMock
            .Setup(x => x.GetById<User>(It.IsAny<int>()))
            .Returns(Task.FromResult<User?>(null));

        var createUserWatchlistCommandHandler = new CreateUserWatchlistCommandHandler(repositoryMock.Object, mapperMock.Object);

        await Assert.ThrowsAsync<EntityNotFoundException>(async () => await createUserWatchlistCommandHandler.Handle(userWatchlistCommand, new CancellationToken()));

        mapperMock.Verify(x => x.Map<CreateUserWatchlistCommand, UserWatchlist>(It.IsAny<CreateUserWatchlistCommand>()), Times.Never);

        repositoryMock.Verify(x => x.Add(It.IsAny<UserWatchlist>()), Times.Never);

        repositoryMock.Verify(x => x.SaveChanges(), Times.Never);

        mapperMock.Verify(x => x.Map<UserWatchlist, UserWatchlistDto>(It.IsAny<UserWatchlist>()), Times.Never);
    }

    [Fact]
    public async void CreateUserWatchlistAuctionNotFound()
    {
        var userWatchlistCommand = new CreateUserWatchlistCommand
        {
            UserId = 1,
            AuctionId = 1,
        };

        var user = new User();

        var repositoryMock = new Mock<IRepository>();

        var mapperMock = new Mock<IMapper>();

        repositoryMock
            .Setup(x => x.GetById<User>(It.IsAny<int>()))
            .Returns(Task.FromResult<User?>(user));

        repositoryMock
            .Setup(x => x.GetById<Auction>(It.IsAny<int>()))
            .Returns(Task.FromResult<Auction?>(null));

        var createUserWatchlistCommandHandler = new CreateUserWatchlistCommandHandler(repositoryMock.Object, mapperMock.Object);

        await Assert.ThrowsAsync<EntityNotFoundException>(async () => await createUserWatchlistCommandHandler.Handle(userWatchlistCommand, new CancellationToken()));

        mapperMock.Verify(x => x.Map<CreateUserWatchlistCommand, UserWatchlist>(It.IsAny<CreateUserWatchlistCommand>()), Times.Never);

        repositoryMock.Verify(x => x.Add(It.IsAny<UserWatchlist>()), Times.Never);

        repositoryMock.Verify(x => x.SaveChanges(), Times.Never);

        mapperMock.Verify(x => x.Map<UserWatchlist, UserWatchlistDto>(It.IsAny<UserWatchlist>()), Times.Never);
    }
}
