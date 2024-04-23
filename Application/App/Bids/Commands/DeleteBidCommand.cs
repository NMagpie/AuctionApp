﻿using Application.Abstractions;
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
        var bid = await _repository.GetById<Bid>(request.Id)
            ?? throw new ArgumentNullException("Bid cannot be found");

        if (bid.Lot.Auction.StartTime <= DateTime.UtcNow + TimeSpan.FromMinutes(5))
        {
            throw new ArgumentException("Cannot edit lots of auction 5 minutes before its start");
        }

        bid = await _repository.Remove<Bid>(request.Id);

        await _repository.SaveChanges();

        var bidDto = BidDto.FromBid(bid);

        return bidDto;
    }
}
