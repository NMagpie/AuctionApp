using Application.App.Queries;
using Application.App.Users.Responses;
using Application.Common.Abstractions;
using Application.Common.Exceptions;
using AutoMapper;
using Domain.Auth;
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

        var getUserByIdQueryHandler = new GetUserByIdQueryHandler(userRepositoryMock.Object, mapperMock.Object);

        var result = await getUserByIdQueryHandler.Handle(userCommand, new CancellationToken());

        userRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Once);

        mapperMock.Verify(x => x.Map<User, UserDto>(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public async void GetUserNotFound()
    {
        var userCommand = new GetUserByIdQuery
        {
            Id = 1,
        };

        var userRepositoryMock = new Mock<IUserRepository>();

        var mapperMock = new Mock<IMapper>();

        userRepositoryMock
            .Setup(x => x.GetById(It.IsAny<int>()))
            .Returns(Task.FromResult<User?>(null));

        var getUserByIdQueryHandler = new GetUserByIdQueryHandler(userRepositoryMock.Object, mapperMock.Object);

        await Assert.ThrowsAsync<EntityNotFoundException>(async () => await getUserByIdQueryHandler.Handle(userCommand, new CancellationToken()));

        userRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Once);

        mapperMock.Verify(x => x.Map<User, UserDto>(It.IsAny<User>()), Times.Never);
    }
}
