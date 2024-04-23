using Application.Abstractions;
using Application.App.Bids.Responses;
using AuctionApp.Domain.Models;
using MediatR;

namespace Application.App.Bids.Commands;

public class DeleteBidCommand : IRequest<BidDto>
{
    public int Id { get; set; }
}


public class DeleteBidCommandHandler : IRequestHandler<DeleteBidCommand, BidDto>
{

    private readonly IRepository _repository;

    public DeleteBidCommandHandler(IRepository repository)
    {
        _repository = repository;
    }

    public async Task<BidDto> Handle(DeleteBidCommand request, CancellationToken cancellationToken)
    {
        var bid = await _repository.Remove<Bid>(request.Id);

        await _repository.SaveChanges();

        var bidDto = BidDto.FromBid(bid);

        return bidDto;
    }
}
