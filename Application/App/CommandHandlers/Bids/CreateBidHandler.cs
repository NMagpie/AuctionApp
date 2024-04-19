using Application.Abstractions;
using Application.App.Commands.Bids;
using Application.App.Responses;
using AuctionApp.Domain.Enumerators;
using AuctionApp.Domain.Models;
using MediatR;

namespace Application.App.CommandHandlers.Bids;
public class CreateBidHandler : IRequestHandler<CreateBidCommand, BidDto>
{
    private readonly IUnitOfWork _unitofWork;

    private readonly CreateBidCommandValidator _validator;

    public CreateBidHandler(IUnitOfWork unitOfWork) //add auction manager in future
    {
        _unitofWork = unitOfWork;
        _validator = new CreateBidCommandValidator();
    }

    public async Task<BidDto> Handle(CreateBidCommand request, CancellationToken cancellationToken)
    {
        _validator.Validate(request);

        var lot = await _unitofWork.Repository.GetById<Lot>(request.LotId)
            ?? throw new ArgumentNullException("Lot cannot be found");

        var auction = lot.Auction
            ?? throw new ArgumentNullException("Auction cannot be found");

        if (auction.StatusId != (int)AuctionStatusId.Active)
        {
            throw new ArgumentException("Cannot place bid: Auction is not active");
        }

        if (auction.EndTime <= DateTime.UtcNow)
        {
            throw new ArgumentException("Cannot place bid: Auction Time is out");
        }

        // place bid using auction manager and receive bid record.

        Bid bid = null;

        var bidDto = BidDto.FromBid(bid);

        return bidDto;
    }
}
