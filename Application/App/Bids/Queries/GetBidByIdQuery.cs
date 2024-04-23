using Application.Abstractions;
using Application.App.Bids.Responses;
using AuctionApp.Domain.Models;
using MediatR;

namespace Application.App.Queries;
public class GetBidByIdQuery : IRequest<BidDto>
{
    public int Id { get; set; }
}

public class GetBidByIdQueryHandler : IRequestHandler<GetBidByIdQuery, BidDto>
{
    private readonly IRepository _repository;

    public GetBidByIdQueryHandler(IRepository repository)
    {
        _repository = repository;
    }

    public async Task<BidDto> Handle(GetBidByIdQuery request, CancellationToken cancellationToken)
    {
        var auction = await _repository.GetById<Bid>(request.Id)
            ?? throw new ArgumentNullException("Bid cannot be found");

        var auctionDto = BidDto.FromBid(auction);

        return auctionDto;
    }
}
