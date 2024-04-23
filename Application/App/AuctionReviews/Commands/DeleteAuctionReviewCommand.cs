using Application.Abstractions;
using Application.App.AuctionReviews.Responses;
using AuctionApp.Domain.Models;
using MediatR;

namespace Application.App.AuctionReviews.Commands;

public class DeleteAuctionReviewCommand : IRequest<AuctionReviewDto>
{
    public int Id { get; set; }
}

public class DeleteAuctionReviewCommandHandler : IRequestHandler<DeleteAuctionReviewCommand, AuctionReviewDto>
{
    private readonly IRepository _repository;

    public DeleteAuctionReviewCommandHandler(IRepository repository)
    {
        _repository = repository;
    }

    public async Task<AuctionReviewDto> Handle(DeleteAuctionReviewCommand request, CancellationToken cancellationToken)
    {
        var auctionReview = await _repository.Remove<AuctionReview>(request.Id);

        await _repository.SaveChanges();

        var auctionReviewDto = AuctionReviewDto.FromAuctionReview(auctionReview);

        return auctionReviewDto;
    }
}
