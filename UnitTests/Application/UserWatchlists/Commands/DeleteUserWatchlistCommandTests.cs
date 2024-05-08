using Application.App.UserWatchlists.Commands;
using Application.Common.Abstractions;
using AuctionApp.Domain.Models;
using Moq;

namespace UnitTests.Application.UserWatchlists.Commands;
public class DeleteUserWatchlistCommandTests
{
    [Fact]
    public async void DeleteUserWatchlistGood()
    {
        var userWatchlistCommand = new DeleteUserWatchlistCommand
        {
            Id = 1,
        };

        var repositoryMock = new Mock<IRepository>();

        repositoryMock
            .Setup(x => x.Remove<UserWatchlist>(It.IsAny<int>()))
            .Returns(Task.CompletedTask);

        var deleteUserWatchlistCommandHandler = new DeleteUserWatchlistCommandHandler(repositoryMock.Object);

        await deleteUserWatchlistCommandHandler.Handle(userWatchlistCommand, new CancellationToken());

        repositoryMock.Verify(x => x.Remove<UserWatchlist>(It.IsAny<int>()), Times.Once);

        repositoryMock.Verify(x => x.SaveChanges(), Times.Once);
    }
}
