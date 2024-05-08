using Application.App.Users.Commands;
using Application.App.Users.Responses;
using Application.Common.Abstractions;
using Application.Common.Exceptions;
using AuctionApp.Domain.Models;
using AutoMapper;
using Moq;

namespace UnitTests.Application.Users.Commands;
public class UpdateUserCommandTests
{
    [Fact]
    public async void UpdateUserGood()
    {
        var userCommand = new UpdateUserCommand
        {
            Id = 1,
            Username = "Test",
        };

        var user = new User
        {
            Id = 1,
            Username = "OldUsername",
        };

        var userDto = new UserDto
        {
            Id = 1,
            Username = userCommand.Username,
            Balance = user.Balance,
        };

        var repositoryMock = new Mock<IRepository>();

        var mapperMock = new Mock<IMapper>();

        repositoryMock
            .Setup(x => x.GetById<User>(It.IsAny<int>()))
            .Returns(Task.FromResult(user));

        mapperMock
            .Setup(x => x.Map(It.IsAny<UpdateUserCommand>(), It.IsAny<User>()))
            .Returns(user);

        mapperMock
            .Setup(x => x.Map<User, UserDto>(It.IsAny<User>()))
            .Returns(userDto);

        var createUserCommandHandler = new UpdateUserCommandHandler(repositoryMock.Object, mapperMock.Object);

        var result = await createUserCommandHandler.Handle(userCommand, new CancellationToken());

        repositoryMock.Verify(x => x.GetById<User>(It.IsAny<int>()), Times.Once);

        mapperMock.Verify(x => x.Map(It.IsAny<UpdateUserCommand>(), It.IsAny<User>()), Times.Once);

        repositoryMock.Verify(x => x.SaveChanges(), Times.Once);

        mapperMock.Verify(x => x.Map<User, UserDto>(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public async void UpdateUserNotFound()
    {
        var userCommand = new UpdateUserCommand
        {
            Id = 1,
            Username = "Test",
        };

        var repositoryMock = new Mock<IRepository>();

        var mapperMock = new Mock<IMapper>();

        repositoryMock
            .Setup(x => x.GetById<User>(It.IsAny<int>()))
            .Returns(Task.FromResult<User?>(null));

        var createUserCommandHandler = new UpdateUserCommandHandler(repositoryMock.Object, mapperMock.Object);

        await Assert.ThrowsAsync<EntityNotFoundException>(async () => await createUserCommandHandler.Handle(userCommand, new CancellationToken()));

        repositoryMock.Verify(x => x.GetById<User>(It.IsAny<int>()), Times.Once);

        mapperMock.Verify(x => x.Map(It.IsAny<UpdateUserCommand>(), It.IsAny<User>()), Times.Never);

        repositoryMock.Verify(x => x.SaveChanges(), Times.Never);

        mapperMock.Verify(x => x.Map<User, UserDto>(It.IsAny<User>()), Times.Never);
    }
}
