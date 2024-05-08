using Application.Common.Abstractions;
using AuctionApp.Domain.Models;
using MediatR;

namespace Application.App.AuctionReviews.Commands;

public class DeleteAuctionReviewCommand : IRequest
{
    public int Id { get; set; }
}

public class DeleteAuctionReviewCommandHandler : IRequestHandler<DeleteAuctionReviewCommand>
{
    private readonly IRepository _repository;

    public DeleteAuctionReviewCommandHandler(IRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(DeleteAuctionReviewCommand request, CancellationToken cancellationToken)
    {
        await _repository.Remove<AuctionReview>(request.Id);

        await _repository.SaveChanges();
    }
}
