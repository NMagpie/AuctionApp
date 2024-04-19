using Application.Abstractions;
using Application.App.Commands.Bids;
using Application.App.Responses;
using AuctionApp.Domain.Models;
using MediatR;

namespace Application.App.CommandHandlers.Bids;
public class DeleteBidHandler : IRequestHandler<DeleteBidCommand, BidDto>
{

    private readonly IUnitOfWork _unitofWork;

    public DeleteBidHandler(IUnitOfWork unitOfWork)
    {
        _unitofWork = unitOfWork;
    }

    public async Task<BidDto> Handle(DeleteBidCommand request, CancellationToken cancellationToken)
    {
        var bid = await _unitofWork.Repository.Remove<Bid>(request.Id);

        await _unitofWork.SaveChanges();

        var bidDto = BidDto.FromBid(bid);

        return bidDto;
    }
}
