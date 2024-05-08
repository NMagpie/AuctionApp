using Application.App.Users.Commands;
using Application.App.Users.Responses;
using Application.Common.Abstractions;
using Application.Common.Exceptions;
using AuctionApp.Domain.Models;
using AutoMapper;
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
            Username = "Test",
        };

        var userDto = new UserDto
        {
            Id = 1,
            Username = user.Username,
            Balance = user.Balance,
        };

        var repositoryMock = new Mock<IRepository>();

        var mapperMock = new Mock<IMapper>();

        repositoryMock
            .Setup(x => x.GetById<User>(It.IsAny<int>()))
            .Returns(Task.FromResult(user));

        mapperMock
            .Setup(x => x.Map<User, UserDto>(It.IsAny<User>()))
            .Returns(userDto);

        var addUserBalanceCommandHandler = new AddUserBalanceCommandHandler(repositoryMock.Object, mapperMock.Object);

        var result = await addUserBalanceCommandHandler.Handle(userCommand, new CancellationToken());

        repositoryMock.Verify(x => x.GetById<User>(It.IsAny<int>()), Times.Once);

        repositoryMock.Verify(x => x.SaveChanges(), Times.Once);

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

        var repositoryMock = new Mock<IRepository>();

        var mapperMock = new Mock<IMapper>();

        repositoryMock
            .Setup(x => x.GetById<User>(It.IsAny<int>()))
            .Returns(Task.FromResult<User?>(null));

        var addUserBalanceCommandHandler = new AddUserBalanceCommandHandler(repositoryMock.Object, mapperMock.Object);

        await Assert.ThrowsAsync<EntityNotFoundException>(async () => await addUserBalanceCommandHandler.Handle(userCommand, new CancellationToken()));

        repositoryMock.Verify(x => x.GetById<User>(It.IsAny<int>()), Times.Once);

        repositoryMock.Verify(x => x.SaveChanges(), Times.Never);

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

        var repositoryMock = new Mock<IRepository>();

        var mapperMock = new Mock<IMapper>();

        var addUserBalanceCommandHandler = new AddUserBalanceCommandHandler(repositoryMock.Object, mapperMock.Object);

        await Assert.ThrowsAsync<BusinessValidationException>(async () => await addUserBalanceCommandHandler.Handle(userCommand, new CancellationToken()));

        repositoryMock.Verify(x => x.GetById<User>(It.IsAny<int>()), Times.Never);

        repositoryMock.Verify(x => x.SaveChanges(), Times.Never);

        mapperMock.Verify(x => x.Map<User, UserDto>(It.IsAny<User>()), Times.Never);
    }
}
