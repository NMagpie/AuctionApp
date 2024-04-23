using Application.Abstractions;
using Application.App.Auctions.Responses;
using AuctionApp.Domain.Models;
using MediatR;

namespace Application.App.Auctions.Queries;
public class GetAuctionByIdQuery : IRequest<AuctionDto>
{
    public int Id { get; set; }
}

public class GetAuctionByIdQueryHandler : IRequestHandler<GetAuctionByIdQuery, AuctionDto>
{
    private readonly IRepository _repository;

    public GetAuctionByIdQueryHandler(IRepository repository)
    {
        _repository = repository;
    }

    public async Task<AuctionDto> Handle(GetAuctionByIdQuery request, CancellationToken cancellationToken)
    {
        var auction = await _repository.GetById<Auction>(request.Id)
            ?? throw new ArgumentNullException("Auction cannot be found");

        var auctionDto = AuctionDto.FromAuction(auction);

        return auctionDto;
    }
}