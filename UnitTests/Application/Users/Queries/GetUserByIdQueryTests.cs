using Application.App.Queries;
using Application.App.Users.Responses;
using Application.Common.Abstractions;
using Application.Common.Exceptions;
using AuctionApp.Domain.Models;
using AutoMapper;
using Moq;

namespace UnitTests.Application.Users.Queries;
public class GetUserByIdQueryTests
{
    [Fact]
    public async void GetUserGood()
    {
        var userCommand = new GetUserByIdQuery
        {
            Id = 1,
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

        var getUserByIdQueryHandler = new GetUserByIdQueryHandler(repositoryMock.Object, mapperMock.Object);

        var result = await getUserByIdQueryHandler.Handle(userCommand, new CancellationToken());

        repositoryMock.Verify(x => x.GetById<User>(It.IsAny<int>()), Times.Once);

        mapperMock.Verify(x => x.Map<User, UserDto>(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public async void GetUserNotFound()
    {
        var userCommand = new GetUserByIdQuery
        {
            Id = 1,
        };

        var repositoryMock = new Mock<IRepository>();

        var mapperMock = new Mock<IMapper>();

        repositoryMock
            .Setup(x => x.GetById<User>(It.IsAny<int>()))
            .Returns(Task.FromResult<User?>(null));

        var getUserByIdQueryHandler = new GetUserByIdQueryHandler(repositoryMock.Object, mapperMock.Object);

        await Assert.ThrowsAsync<EntityNotFoundException>(async () => await getUserByIdQueryHandler.Handle(userCommand, new CancellationToken()));

        repositoryMock.Verify(x => x.GetById<User>(It.IsAny<int>()), Times.Once);

        mapperMock.Verify(x => x.Map<User, UserDto>(It.IsAny<User>()), Times.Never);
    }
}
