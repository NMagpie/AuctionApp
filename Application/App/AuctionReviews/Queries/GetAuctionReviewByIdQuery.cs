using Application.Abstractions;
using Application.App.AuctionReviews.Responses;
using AuctionApp.Domain.Models;
using MediatR;

namespace Application.App.Queries;
public class GetAuctionReviewByIdQuery : IRequest<AuctionReviewDto>
{
    public int Id { get; set; }
}

public class GetAuctionReviewByIdQueryHandler : IRequestHandler<GetAuctionReviewByIdQuery, AuctionReviewDto>
{
    private readonly IRepository _repository;

    public GetAuctionReviewByIdQueryHandler(IRepository repository)
    {
        _repository = repository;
    }

    public async Task<AuctionReviewDto> Handle(GetAuctionReviewByIdQuery request, CancellationToken cancellationToken)
    {
        var auctionReview = await _repository.GetById<AuctionReview>(request.Id)
            ?? throw new ArgumentNullException("Auction Review cannot be found");

        var auctionReviewDto = AuctionReviewDto.FromAuctionReview(auctionReview);

        return auctionReviewDto;
    }
}