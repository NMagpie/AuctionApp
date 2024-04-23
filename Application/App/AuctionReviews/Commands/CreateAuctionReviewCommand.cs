using Application.Abstractions;
using Application.App.AuctionReviews.Responses;
using AuctionApp.Domain.Models;
using MediatR;

namespace Application.App.AuctionReviews.Commands;

public class CreateAuctionReviewCommand : IRequest<AuctionReviewDto>
{
    public int UserId { get; set; }

    public int AuctionId { get; set; }

    public string? ReviewText { get; set; }

    public float Rating { get; set; }
}

public class CreateAuctionReviewCommandHandler : IRequestHandler<CreateAuctionReviewCommand, AuctionReviewDto>
{

    private readonly IRepository _repository;

    private readonly CreateAuctionReviewCommandValidator _validator;

    public CreateAuctionReviewCommandHandler(IRepository repository)
    {
        _repository = repository;
        _validator = new CreateAuctionReviewCommandValidator();
    }

    public async Task<AuctionReviewDto> Handle(CreateAuctionReviewCommand request, CancellationToken cancellationToken)
    {
        _validator.Validate(request);

        var user = await _repository.GetById<User>(request.UserId)
            ?? throw new ArgumentNullException("User cannot be found");

        var auction = await _repository.GetById<Auction>(request.AuctionId)
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

        await _repository.Add(auctionReview);

        await _repository.SaveChanges();

        var auctionReviewDto = AuctionReviewDto.FromAuctionReview(auctionReview);

        return auctionReviewDto;
    }
}
