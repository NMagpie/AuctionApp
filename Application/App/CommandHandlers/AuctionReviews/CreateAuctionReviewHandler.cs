using Application.Abstractions;
using Application.App.Commands.AuctionReviews;
using Application.App.Responses;
using AuctionApp.Domain.Models;
using FluentValidation;
using MediatR;

namespace Application.App.CommandHandlers.AuctionReviews;
public class CreateAuctionReviewHandler : IRequestHandler<CreateAuctionReviewCommand, AuctionReviewDto>
{

    private readonly IUnitOfWork _unitofWork;

    private readonly CreateAuctionReviewCommandValidator _validator;

    public CreateAuctionReviewHandler(IUnitOfWork unitOfWork)
    {
        _unitofWork = unitOfWork;
        _validator = new CreateAuctionReviewCommandValidator();
    }

    public async Task<AuctionReviewDto> Handle(CreateAuctionReviewCommand request, CancellationToken cancellationToken)
    {
        _validator.ValidateAndThrow(request);

        var user = await _unitofWork.Repository.GetById<User>(request.UserId)
            ?? throw new ArgumentNullException("User cannot be found");

        var auction = await _unitofWork.Repository.GetById<Auction>(request.AuctionId)
            ?? throw new ArgumentNullException("Auction cannot be found");

        var auctionReview = new AuctionReview
        {
            UserId = request.UserId,
            User = user,
            AuctionId = request.AuctionId,
            Auction = auction,
            ReviewText = request.ReviewText,
            Rating = request.Rating,
        };

        await _unitofWork.Repository.Add(auctionReview);

        await _unitofWork.SaveChanges();

        var auctionReviewDto = AuctionReviewDto.FromAuctionReview(auctionReview);

        return auctionReviewDto;
    }
}
