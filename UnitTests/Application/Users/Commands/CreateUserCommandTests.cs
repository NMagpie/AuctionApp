using Application.App.Users.Commands;
using Application.App.Users.Responses;
using Application.Common.Abstractions;
using AuctionApp.Domain.Models;
using AutoMapper;
using Moq;

namespace UnitTests.Application.Users.Commands;
public class CreateUserCommandTests
{
    [Fact]
    public async void CreateUserGood()
    {
        var userCommand = new CreateUserCommand
        {
            Username = "Test",
        };

        var user = new User
        {
            Id = 1,
            Username = userCommand.Username,
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
            .Setup(x => x.Add(It.IsAny<User>()))
            .Returns(Task.FromResult(user));

        mapperMock
            .Setup(x => x.Map<CreateUserCommand, User>(It.IsAny<CreateUserCommand>()))
            .Returns(user);

        mapperMock
            .Setup(x => x.Map<User, UserDto>(It.IsAny<User>()))
            .Returns(userDto);

        var createUserCommandHandler = new CreateUserCommandHandler(repositoryMock.Object, mapperMock.Object);

        var result = await createUserCommandHandler.Handle(userCommand, new CancellationToken());

        mapperMock.Verify(x => x.Map<CreateUserCommand, User>(It.IsAny<CreateUserCommand>()), Times.Once);

        repositoryMock.Verify(x => x.Add(It.IsAny<User>()), Times.Once);

        repositoryMock.Verify(x => x.SaveChanges(), Times.Once);

        mapperMock.Verify(x => x.Map<User, UserDto>(It.IsAny<User>()), Times.Once);
    }
}
