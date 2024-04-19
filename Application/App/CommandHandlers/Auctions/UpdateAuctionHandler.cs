using Application.Abstractions;
using Application.App.Commands.Auctions;
using Application.App.Responses;
using AuctionApp.Domain.Enumerators;
using AuctionApp.Domain.Models;
using MediatR;

namespace Application.App.CommandHandlers.Auctions;
public class UpdateAuctionHandler : IRequestHandler<UpdateAuctionCommand, AuctionDto>
{
    private readonly IUnitOfWork _unitofWork;

    private readonly UpdateAuctionCommandValidator _validator;

    public UpdateAuctionHandler(IUnitOfWork unitOfWork)
    {
        _unitofWork = unitOfWork;
        _validator = new UpdateAuctionCommandValidator();
    }

    public async Task<AuctionDto> Handle(UpdateAuctionCommand request, CancellationToken cancellationToken)
    {
        _validator.Validate(request);

        var auction = await _unitofWork.Repository.GetById<Auction>(request.Id)
            ?? throw new ArgumentNullException("Auction cannot be found");

        if (auction.StatusId != (int)AuctionStatusId.Created)
        {
            throw new ArgumentException("Cannot update started or finished auction");
        }

        auction.Title = request.Title ?? auction.Title;
        auction.StartTime = request.StartTime ?? auction.StartTime;
        auction.EndTime = request.EndTime ?? auction.EndTime;

        var lots = request.LotIds
            ?.Select(async lotId => await _unitofWork.Repository.GetById<Lot>(lotId))
            .Select(t => t.Result)
            .Select((lot, i) => { lot.LotOrder = i; return lot; })
            .ToList();

        auction.Lots = lots ?? [];

        await _unitofWork.SaveChanges();

        var auctionDto = AuctionDto.FromAuction(auction);

        return auctionDto;
    }
}
