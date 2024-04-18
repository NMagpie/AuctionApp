using Application.Abstractions;
using Application.App.Commands.AuctionReviews;
using Application.App.Responses;
using AuctionApp.Domain.Models;
using FluentValidation;
using MediatR;

namespace Application.App.CommandHandlers.AuctionReviews;
public class UpdateAuctionReviewHandler : IRequestHandler<UpdateAuctionReviewCommand, AuctionReviewDto>
{
    private readonly IUnitOfWork _unitofWork;

    private readonly UpdateAuctionReviewCommandValidator _validator;

    public UpdateAuctionReviewHandler(IUnitOfWork unitOfWork)
    {
        _unitofWork = unitOfWork;
        _validator = new UpdateAuctionReviewCommandValidator();
    }

    public async Task<AuctionReviewDto> Handle(UpdateAuctionReviewCommand request, CancellationToken cancellationToken)
    {
        _validator.ValidateAndThrow(request);

        var auctionReview = await _unitofWork.Repository.GetById<AuctionReview>(request.Id)
            ?? throw new ArgumentNullException("AuctionReview cannot be found");

        auctionReview.ReviewText = request.ReviewText ?? auctionReview.ReviewText;

        auctionReview.Rating = request.Rating ?? auctionReview.Rating;

        await _unitofWork.SaveChanges();

        var auctionReviewDto = AuctionReviewDto.FromAuctionReview(auctionReview);

        return auctionReviewDto;
    }
}
