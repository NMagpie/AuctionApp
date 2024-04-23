using Application.Abstractions;
using Application.App.Auctions.Responses;
using AuctionApp.Domain.Enumerators;
using AuctionApp.Domain.Models;
using MediatR;

namespace Application.App.Auctions.Commands;

public class DeleteAuctionCommand : IRequest<AuctionDto>
{
    public int Id { get; set; }
}

public class DeleteAuctionCommandHandler : IRequestHandler<DeleteAuctionCommand, AuctionDto>
{
    private readonly IRepository _repository;

    public DeleteAuctionCommandHandler(IRepository repository)
    {
        _repository = repository;
    }

    public async Task<AuctionDto> Handle(DeleteAuctionCommand request, CancellationToken cancellationToken)
    {
        var auction = await _repository.GetById<Auction>(request.Id)
            ?? throw new ArgumentNullException("Auction cannot be found");

        if (auction.StatusId != (int)AuctionStatusId.Created)
        {
            throw new ArgumentException("Cannot edit started auction");
        }

        auction = await _repository.Remove<Auction>(request.Id);

        await _repository.SaveChanges();

        var auctionDto = AuctionDto.FromAuction(auction);

        return auctionDto;
    }
}
