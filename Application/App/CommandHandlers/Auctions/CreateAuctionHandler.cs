using Application.Abstractions;
using Application.App.Commands.Auctions;
using Application.App.Responses;
using AuctionApp.Domain.Enumerators;
using AuctionApp.Domain.Models;
using EntityFramework.Domain.Models;
using MediatR;

namespace Application.App.CommandHandlers.Auctions;
public class CreateAuctionHandler : IRequestHandler<CreateAuctionCommand, AuctionDto>
{
    private readonly IUnitOfWork _unitofWork;

    private readonly CreateAuctionCommandValidator _validator;

    public CreateAuctionHandler(IUnitOfWork unitOfWork)
    {
        _unitofWork = unitOfWork;
        _validator = new CreateAuctionCommandValidator();
    }

    public async Task<AuctionDto> Handle(CreateAuctionCommand request, CancellationToken cancellationToken)
    {
        _validator.Validate(request);

        var user = await _unitofWork.Repository.GetById<User>(request.CreatorId)
            ?? throw new ArgumentNullException("User cannot be found");

        var statusId = (int)AuctionStatusId.Created;

        var auctionStatus = await _unitofWork.Repository.GetById<AuctionStatus>(statusId);

        var lots = request.LotIds
            .Select(async lotId => await _unitofWork.Repository.GetById<Lot>(lotId))
            .Select(t => t.Result)
            .Select((lot, i) => { lot.LotOrder = i; return lot; })
            .ToList();

        var auction = new Auction()
        {
            Title = request.Title,
            CreatorId = request.CreatorId,
            Creator = user,
            StartTime = request.StartTime,
            EndTime = request.EndTime,
            StatusId = statusId,
            Status = auctionStatus,
            Lots = lots
        };

        var auctionDto = AuctionDto.FromAuction(auction);

        return auctionDto;
    }
}
