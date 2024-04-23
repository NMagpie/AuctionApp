using Application.Abstractions;
using Application.App.AuctionReviews.Responses;
using AuctionApp.Domain.Models;
using FluentValidation;
using MediatR;

namespace Application.App.AuctionReviews.Commands;

public class UpdateAuctionReviewCommand : IRequest<AuctionReviewDto>
{
    public int Id { get; set; }

    public string? ReviewText { get; set; }

    public float? Rating { get; set; }
}

public class UpdateAuctionReviewCommandHandler : IRequestHandler<UpdateAuctionReviewCommand, AuctionReviewDto>
{
    private readonly IRepository _repository;

    private readonly UpdateAuctionReviewCommandValidator _validator;

    public UpdateAuctionReviewCommandHandler(IRepository repository)
    {
        _repository = repository;
        _validator = new UpdateAuctionReviewCommandValidator();
    }

    public async Task<AuctionReviewDto> Handle(UpdateAuctionReviewCommand request, CancellationToken cancellationToken)
    {
        _validator.ValidateAndThrow(request);

        var auctionReview = await _repository.GetById<AuctionReview>(request.Id)
            ?? throw new ArgumentNullException("AuctionReview cannot be found");

        auctionReview.ReviewText = request.ReviewText ?? auctionReview.ReviewText;

        auctionReview.Rating = request.Rating ?? auctionReview.Rating;

        await _repository.SaveChanges();

        var auctionReviewDto = AuctionReviewDto.FromAuctionReview(auctionReview);

        return auctionReviewDto;
    }
}
