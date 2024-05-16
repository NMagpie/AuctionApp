using Application.App.Users.Commands;
using Application.App.Users.Responses;
using Application.Common.Abstractions;
using Application.Common.Exceptions;
using AutoMapper;
using Domain.Auth;
using Moq;

namespace UnitTests.Application.Users.Commands;
public class AddUserBalanceCommandTests
{
    [Fact]
    public async void AddUserBalanceGood()
    {
        var userCommand = new AddUserBalanceCommand
        {
            Id = 1,
            Amount = 10,
        };

        var user = new User
        {
            Id = 1,
            UserName = "Test",
        };

        var userDto = new UserDto
        {
            Id = 1,
            UserName = user.UserName,
            Balance = user.Balance,
        };

        var userRepositoryMock = new Mock<IUserRepository>();

        var mapperMock = new Mock<IMapper>();

        userRepositoryMock
            .Setup(x => x.GetById(It.IsAny<int>()))
            .Returns(Task.FromResult(user));

        mapperMock
            .Setup(x => x.Map<User, UserDto>(It.IsAny<User>()))
            .Returns(userDto);

        var addUserBalanceCommandHandler = new AddUserBalanceCommandHandler(userRepositoryMock.Object, mapperMock.Object);

        var result = await addUserBalanceCommandHandler.Handle(userCommand, new CancellationToken());

        userRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Once);

        userRepositoryMock.Verify(x => x.SaveChanges(), Times.Once);

        mapperMock.Verify(x => x.Map<User, UserDto>(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public async void AddUserBalanceUserNotFound()
    {
        var userCommand = new AddUserBalanceCommand
        {
            Id = 1,
            Amount = 10,
        };

        var userRepositoryMock = new Mock<IUserRepository>();

        var mapperMock = new Mock<IMapper>();

        userRepositoryMock
            .Setup(x => x.GetById(It.IsAny<int>()))
            .Returns(Task.FromResult<User?>(null));

        var addUserBalanceCommandHandler = new AddUserBalanceCommandHandler(userRepositoryMock.Object, mapperMock.Object);

        await Assert.ThrowsAsync<EntityNotFoundException>(async () => await addUserBalanceCommandHandler.Handle(userCommand, new CancellationToken()));

        userRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Once);

        userRepositoryMock.Verify(x => x.SaveChanges(), Times.Never);

        mapperMock.Verify(x => x.Map<User, UserDto>(It.IsAny<User>()), Times.Never);
    }

    [Fact]
    public async void AddUserBalanceNotPositiveAmount()
    {
        var userCommand = new AddUserBalanceCommand
        {
            Id = 1,
            Amount = 0,
        };

        var userRepositoryMock = new Mock<IUserRepository>();

        var mapperMock = new Mock<IMapper>();

        var addUserBalanceCommandHandler = new AddUserBalanceCommandHandler(userRepositoryMock.Object, mapperMock.Object);

        await Assert.ThrowsAsync<BusinessValidationException>(async () => await addUserBalanceCommandHandler.Handle(userCommand, new CancellationToken()));

        userRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Never);

        userRepositoryMock.Verify(x => x.SaveChanges(), Times.Never);

        mapperMock.Verify(x => x.Map<User, UserDto>(It.IsAny<User>()), Times.Never);
    }
}
