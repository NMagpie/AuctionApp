using Application.App.Queries;
using Application.App.Users.Responses;
using Application.App.UserWatchlists.Responses;
using Application.Common.Abstractions;
using Application.Common.Exceptions;
using AuctionApp.Domain.Models;
using AutoMapper;
using Moq;

namespace UnitTests.Application.UserWatchlists.Queries;
public class GetUserWatchlistByIdQueryTests
{
    [Fact]
    public async void GetUserWatchlistGood()
    {
        var userWatchlistCommand = new GetUserWatchlistByIdQuery
        {
            Id = 1,
        };

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

        var repositoryMock = new Mock<IEntityRepository>();

        var mapperMock = new Mock<IMapper>();

        repositoryMock
            .Setup(x => x.GetById<UserWatchlist>(It.IsAny<int>()))
            .Returns(Task.FromResult(userWatchlist));

        mapperMock
            .Setup(x => x.Map<UserWatchlist, UserWatchlistDto>(It.IsAny<UserWatchlist>()))
            .Returns(userWatchlistDto);

        var getUserWatchlistByIdQueryHandler = new GetUserWatchlistByIdQueryHandler(repositoryMock.Object, mapperMock.Object);

        var result = await getUserWatchlistByIdQueryHandler.Handle(userWatchlistCommand, new CancellationToken());

        repositoryMock.Verify(x => x.GetById<UserWatchlist>(It.IsAny<int>()), Times.Once);

        mapperMock.Verify(x => x.Map<UserWatchlist, UserWatchlistDto>(It.IsAny<UserWatchlist>()), Times.Once);
    }

    [Fact]
    public async void GetUserWatchlistNotFound()
    {
        var userWatchlistCommand = new GetUserWatchlistByIdQuery
        {
            Id = 1,
        };

        var repositoryMock = new Mock<IEntityRepository>();

        var mapperMock = new Mock<IMapper>();

        repositoryMock
            .Setup(x => x.GetById<UserWatchlist>(It.IsAny<int>()))
            .Returns(Task.FromResult<UserWatchlist?>(null));

        var getUserWatchlistByIdQueryHandler = new GetUserWatchlistByIdQueryHandler(repositoryMock.Object, mapperMock.Object);

        await Assert.ThrowsAsync<EntityNotFoundException>(async () => await getUserWatchlistByIdQueryHandler.Handle(userWatchlistCommand, new CancellationToken()));

        repositoryMock.Verify(x => x.GetById<UserWatchlist>(It.IsAny<int>()), Times.Once);

        mapperMock.Verify(x => x.Map<UserWatchlist, UserWatchlistDto>(It.IsAny<UserWatchlist>()), Times.Never);
    }
}
