using Application.Abstractions;
using Application.App.Commands.Auctions;
using Application.App.Responses;
using AuctionApp.Domain.Models;
using MediatR;

namespace Application.App.CommandHandlers.Auctions;
public class DeleteAuctionHandler : IRequestHandler<DeleteAuctionCommand, AuctionDto>
{
    private readonly IUnitOfWork _unitofWork;

    public DeleteAuctionHandler(IUnitOfWork unitOfWork)
    {
        _unitofWork = unitOfWork;
    }

    public async Task<AuctionDto> Handle(DeleteAuctionCommand request, CancellationToken cancellationToken)
    {
        var auction = await _unitofWork.Repository.Remove<Auction>(request.Id);

        await _unitofWork.SaveChanges();

        var auctionDto = AuctionDto.FromAuction(auction);

        return auctionDto;
    }
}
