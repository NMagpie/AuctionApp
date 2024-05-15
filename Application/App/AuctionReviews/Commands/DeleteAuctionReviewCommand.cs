using Application.Common.Abstractions;
using Application.Common.Exceptions;
using AuctionApp.Domain.Models;
using MediatR;

namespace Application.App.AuctionReviews.Commands;

public class DeleteAuctionReviewCommand : IRequest
{
    public int Id { get; set; }

    public int UserId { get; set; }
}

public class DeleteAuctionReviewCommandHandler : IRequestHandler<DeleteAuctionReviewCommand>
{
    private readonly IEntityRepository _repository;

    public DeleteAuctionReviewCommandHandler(IEntityRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(DeleteAuctionReviewCommand request, CancellationToken cancellationToken)
    {
        var auctionReview = await _repository.GetById<AuctionReview>(request.Id)
            ?? throw new EntityNotFoundException("Auction Review cannot be found");

        if (auctionReview.UserId != request.UserId)
        {
            throw new InvalidUserException("You do not have permission to modify this data");
        }

        await _repository.Remove<AuctionReview>(request.Id);

        await _repository.SaveChanges();
    }
}
