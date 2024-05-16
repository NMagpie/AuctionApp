using Application.Common.Abstractions;
using Application.Common.Exceptions;
using AuctionApp.Domain.Models;
using MediatR;

namespace Application.App.Bids.Commands;

public class DeleteBidCommand : IRequest
{
    public int Id { get; set; }
}


public class DeleteBidCommandHandler : IRequestHandler<DeleteBidCommand>
{

    private readonly IEntityRepository _repository;

    public DeleteBidCommandHandler(IEntityRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(DeleteBidCommand request, CancellationToken cancellationToken)
    {
        var bid = await _repository.GetByIdWithInclude<Bid>(request.Id, bid => bid.Lot, bid => bid.Lot.Auction)
            ?? throw new EntityNotFoundException("Bid cannot be found");

        if (bid.Lot.Auction.EndTime <= DateTimeOffset.UtcNow)
        {
            throw new BusinessValidationException("Cannot remove bid: Auction Time is out");
        }

        await _repository.Remove<Bid>(request.Id);

        await _repository.SaveChanges();
    }
}
