using Application.App.Users.Commands;
using Application.Common.Abstractions;
using Domain.Auth;
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
            UserName = "Test",
        };

        var userRepositoryMock = new Mock<IUserRepository>();

        userRepositoryMock
            .Setup(x => x.Add(It.IsAny<User>()))
            .Returns(Task.FromResult(user));

        var createUserCommandHandler = new DeleteUserCommandHandler(userRepositoryMock.Object);

        await createUserCommandHandler.Handle(userCommand, new CancellationToken());

        userRepositoryMock.Verify(x => x.Remove(It.IsAny<int>()), Times.Once);

        userRepositoryMock.Verify(x => x.SaveChanges(), Times.Once);
    }
}
