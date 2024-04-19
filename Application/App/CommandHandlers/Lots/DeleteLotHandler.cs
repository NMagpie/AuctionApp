using Application.Abstractions;
using Application.App.Commands.Lots;
using Application.App.Responses;
using AuctionApp.Domain.Enumerators;
using AuctionApp.Domain.Models;
using MediatR;

namespace Application.App.CommandHandlers.Lots;
public class DeleteLotHandler : IRequestHandler<DeleteLotCommand, LotDto>
{
    private readonly IUnitOfWork _unitofWork;

    public DeleteLotHandler(IUnitOfWork unitOfWork)
    {
        _unitofWork = unitOfWork;
    }
    public async Task<LotDto> Handle(DeleteLotCommand request, CancellationToken cancellationToken)
    {
        var lot = await _unitofWork.Repository.GetById<Lot>(request.Id)
            ?? throw new ArgumentNullException("Lot cannot be found");

        if (lot.Auction?.StatusId != (int)AuctionStatusId.Created)
        {
            throw new ArgumentException("Cannot edit lot of started auction");
        }

        lot = await _unitofWork.Repository.Remove<Lot>(request.Id);

        await _unitofWork.SaveChanges();

        var lotDto = LotDto.FromLot(lot);

        return lotDto;
    }
}
