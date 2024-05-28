using Application.App.Bids.Responses;
using Application.Common.Abstractions;
using Application.Common.Exceptions;
using AuctionApp.Domain.Models;
using AutoMapper;
using MediatR;

namespace Application.App.Bids.Commands;

public class CreateBidCommand : IRequest<BidDto>
{
    public required int LotId { get; set; }

    public required int UserId { get; set; }

    public required decimal Amount { get; set; }
}

public class CreateBidCommandHandler : IRequestHandler<CreateBidCommand, BidDto>
{
    private readonly IEntityRepository _repository;

    private readonly IMapper _mapper;

    public CreateBidCommandHandler(IEntityRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<BidDto> Handle(CreateBidCommand request, CancellationToken cancellationToken)
    {
        var lot = await _repository.GetByIdWithInclude<Lot>(request.LotId, lot => lot.Auction)
            ?? throw new EntityNotFoundException("Lot cannot be found");

        if (lot.Auction.EndTime <= DateTimeOffset.UtcNow)
        {
            throw new BusinessValidationException("Cannot place bid: Auction Time is out");
        }

        if (lot.Bids.DefaultIfEmpty().Max(bid => bid.Amount) >= request.Amount)
        {
            throw new BusinessValidationException("Cannot place bid: someone placed greater bid");
        }

        var bid = _mapper.Map<CreateBidCommand, Bid>(request);

        bid.CreateTime = DateTimeOffset.UtcNow;

        await _repository.Add(bid);

        await _repository.SaveChanges();

        var bidDto = _mapper.Map<Bid, BidDto>(bid);

        return bidDto;
    }
}
