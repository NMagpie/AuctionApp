using Application.App.AuctionReviews.Commands;
using Application.Common.Abstractions;
using Application.Common.Exceptions;
using AuctionApp.Domain.Models;
using AutoMapper;
using Domain.Auth;
using Moq;

namespace UnitTests.Application.AuctionReviews.Commands;
public class CreateAuctionReviewCommandTests
{
    [Fact]
    public async void CreateAuctionReviewGood()
    {
        var auctionReviewCommand = new CreateAuctionReviewCommand
        {
            UserId = 1,
            AuctionId = 1,
            ReviewText = "123",
            Rating = 2,
        };

        var auctionReview = new AuctionReview
        {
            UserId = auctionReviewCommand.UserId,
            AuctionId = auctionReviewCommand.AuctionId,
            ReviewText = auctionReviewCommand.ReviewText,
            Rating = auctionReviewCommand.Rating,
        };

        var user = new User
        {
            Id = auctionReviewCommand.UserId,
        };

        var auction = new Auction
        {
            Id = auctionReview.AuctionId,
            EndTime = DateTime.Today,
        };

        var repositoryMock = new Mock<IEntityRepository>();

        var userRepositoryMock = new Mock<IUserRepository>();

        var mapperMock = new Mock<IMapper>();

        userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(Task.FromResult<User?>(user));

        repositoryMock.Setup(x => x.GetById<Auction>(It.IsAny<int>())).Returns(Task.FromResult<Auction?>(auction));

        repositoryMock.Setup(x => x.Add(It.IsAny<AuctionReview>())).Returns(Task.FromResult(auctionReview));

        var createAuctionReviewHandler = new CreateAuctionReviewCommandHandler(repositoryMock.Object, userRepositoryMock.Object, mapperMock.Object);

        var result = await createAuctionReviewHandler.Handle(auctionReviewCommand, new CancellationToken());

        userRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Once);

        repositoryMock.Verify(x => x.GetById<Auction>(It.IsAny<int>()), Times.Once);

        mapperMock.Verify(x => x.Map<CreateAuctionReviewCommand, AuctionReview>(It.IsAny<CreateAuctionReviewCommand>()), Times.Once);

        repositoryMock.Verify(x => x.SaveChanges(), Times.Once);

        repositoryMock.Verify(x => x.Add(It.IsAny<AuctionReview>()), Times.Once);
    }

    [Fact]
    public async void CreateAuctionReviewUserNotFound()
    {
        var auctionReview = new CreateAuctionReviewCommand
        {
            UserId = 1,
            AuctionId = 1,
            ReviewText = "123",
            Rating = 2,
        };

        var repositoryMock = new Mock<IEntityRepository>();

        var userRepositoryMock = new Mock<IUserRepository>();

        var mapperMock = new Mock<IMapper>();

        userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(Task.FromResult<User?>(null));

        var createAuctionReviewHandler = new CreateAuctionReviewCommandHandler(repositoryMock.Object, userRepositoryMock.Object, mapperMock.Object);

        await Assert.ThrowsAsync<EntityNotFoundException>(async () => await createAuctionReviewHandler.Handle(auctionReview, new CancellationToken()));

        userRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Once);

        repositoryMock.Verify(x => x.GetById<Auction>(It.IsAny<int>()), Times.Never);

        mapperMock.Verify(x => x.Map<CreateAuctionReviewCommand, AuctionReview>(It.IsAny<CreateAuctionReviewCommand>()), Times.Never);

        repositoryMock.Verify(x => x.Add(It.IsAny<AuctionReview>()), Times.Never);
    }

    [Fact]
    public async void CreateAuctionReviewAuctionNotFound()
    {
        var auctionReview = new CreateAuctionReviewCommand
        {
            UserId = 1,
            AuctionId = 1,
            ReviewText = "123",
            Rating = 2,
        };

        var user = new User
        {
            Id = auctionReview.UserId,
        };

        var repositoryMock = new Mock<IEntityRepository>();

        var userRepositoryMock = new Mock<IUserRepository>();

        var mapperMock = new Mock<IMapper>();

        userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(Task.FromResult<User?>(user));

        repositoryMock.Setup(x => x.GetById<Auction>(It.IsAny<int>())).Returns(Task.FromResult<Auction?>(null));

        var createAuctionReviewHandler = new CreateAuctionReviewCommandHandler(repositoryMock.Object, userRepositoryMock.Object, mapperMock.Object);

        await Assert.ThrowsAsync<EntityNotFoundException>(async () => await createAuctionReviewHandler.Handle(auctionReview, new CancellationToken()));

        userRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Once);

        repositoryMock.Verify(x => x.GetById<Auction>(It.IsAny<int>()), Times.Once);

        mapperMock.Verify(x => x.Map<CreateAuctionReviewCommand, AuctionReview>(It.IsAny<CreateAuctionReviewCommand>()), Times.Never);

        repositoryMock.Verify(x => x.Add(It.IsAny<AuctionReview>()), Times.Never);
    }

    [Fact]
    public async void CreateAuctionReviewFinishedAuction()
    {
        var auctionReview = new CreateAuctionReviewCommand
        {
            UserId = 1,
            AuctionId = 1,
            ReviewText = "123",
            Rating = 2,
        };

        var user = new User
        {
            Id = auctionReview.UserId,
        };

        var auction = new Auction
        {
            Id = auctionReview.AuctionId,
            EndTime = DateTime.MaxValue,
        };

        var repositoryMock = new Mock<IEntityRepository>();

        var userRepositoryMock = new Mock<IUserRepository>();

        var mapperMock = new Mock<IMapper>();

        userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(Task.FromResult<User?>(user));

        repositoryMock.Setup(x => x.GetById<Auction>(It.IsAny<int>())).Returns(Task.FromResult<Auction?>(auction));

        var createAuctionReviewHandler = new CreateAuctionReviewCommandHandler(repositoryMock.Object, userRepositoryMock.Object, mapperMock.Object);

        await Assert.ThrowsAsync<BusinessValidationException>(async () => await createAuctionReviewHandler.Handle(auctionReview, new CancellationToken()));

        userRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Once);

        repositoryMock.Verify(x => x.GetById<Auction>(It.IsAny<int>()), Times.Once);

        mapperMock.Verify(x => x.Map<CreateAuctionReviewCommand, AuctionReview>(It.IsAny<CreateAuctionReviewCommand>()), Times.Never);

        repositoryMock.Verify(x => x.Add(It.IsAny<AuctionReview>()), Times.Never);
    }
}