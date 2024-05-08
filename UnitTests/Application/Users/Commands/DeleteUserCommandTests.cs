using Application.App.Users.Commands;
using Application.App.Users.Responses;
using Application.Common.Abstractions;
using AuctionApp.Domain.Models;
using AutoMapper;
using Moq;

namespace UnitTests.Application.Users.Commands;
public class DeleteUserCommandTests
{
    [Fact]
    public async void DeleteUserGood()
    {
        var userCommand = new DeleteUserCommand
        {
            Id = 1,
        };

        var user = new User
        {
            Id = 1,
            Username = "Test",
        };

        var repositoryMock = new Mock<IRepository>();

        repositoryMock
            .Setup(x => x.Add(It.IsAny<User>()))
            .Returns(Task.FromResult(user));

        var createUserCommandHandler = new DeleteUserCommandHandler(repositoryMock.Object);

        await createUserCommandHandler.Handle(userCommand, new CancellationToken());

        repositoryMock.Verify(x => x.Remove<User>(It.IsAny<int>()), Times.Once);

        repositoryMock.Verify(x => x.SaveChanges(), Times.Once);
    }
}
