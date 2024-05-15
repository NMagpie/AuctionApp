using Application.App.Users.Commands;
using Application.App.Users.Responses;
using Application.Common.Abstractions;
using Application.Common.Exceptions;
using AutoMapper;
using Domain.Auth;
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
            UserName = "Test",
        };

        var user = new User
        {
            Id = 1,
            UserName = "OldUsername",
        };

        var userDto = new UserDto
        {
            Id = 1,
            UserName = userCommand.UserName,
            Balance = user.Balance,
        };

        var userRepositoryMock = new Mock<IUserRepository>();

        var mapperMock = new Mock<IMapper>();

        userRepositoryMock
            .Setup(x => x.GetById(It.IsAny<int>()))
            .Returns(Task.FromResult(user));

        mapperMock
            .Setup(x => x.Map(It.IsAny<UpdateUserCommand>(), It.IsAny<User>()))
            .Returns(user);

        mapperMock
            .Setup(x => x.Map<User, UserDto>(It.IsAny<User>()))
            .Returns(userDto);

        var createUserCommandHandler = new UpdateUserCommandHandler(userRepositoryMock.Object, mapperMock.Object);

        var result = await createUserCommandHandler.Handle(userCommand, new CancellationToken());

        userRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Once);

        mapperMock.Verify(x => x.Map(It.IsAny<UpdateUserCommand>(), It.IsAny<User>()), Times.Once);

        userRepositoryMock.Verify(x => x.SaveChanges(), Times.Once);

        mapperMock.Verify(x => x.Map<User, UserDto>(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public async void UpdateUserNotFound()
    {
        var userCommand = new UpdateUserCommand
        {
            Id = 1,
            UserName = "Test",
        };

        var userRepositoryMock = new Mock<IUserRepository>();

        var mapperMock = new Mock<IMapper>();

        userRepositoryMock
            .Setup(x => x.GetById(It.IsAny<int>()))
            .Returns(Task.FromResult<User?>(null));

        var createUserCommandHandler = new UpdateUserCommandHandler(userRepositoryMock.Object, mapperMock.Object);

        await Assert.ThrowsAsync<EntityNotFoundException>(async () => await createUserCommandHandler.Handle(userCommand, new CancellationToken()));

        userRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Once);

        mapperMock.Verify(x => x.Map(It.IsAny<UpdateUserCommand>(), It.IsAny<User>()), Times.Never);

        userRepositoryMock.Verify(x => x.SaveChanges(), Times.Never);

        mapperMock.Verify(x => x.Map<User, UserDto>(It.IsAny<User>()), Times.Never);
    }
}
