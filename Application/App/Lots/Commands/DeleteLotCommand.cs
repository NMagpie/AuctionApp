using Application.Abstractions;
using Application.App.Lots.Responses;
using AuctionApp.Domain.Enumerators;
using AuctionApp.Domain.Models;
using MediatR;

namespace Application.App.Lots.Commands;

public class DeleteLotCommand : IRequest<LotDto>
{
    public int Id { get; set; }
}

public class DeleteLotCommandHandler : IRequestHandler<DeleteLotCommand, LotDto>
{
    private readonly IRepository _repository;

    public DeleteLotCommandHandler(IRepository repository)
    {
        _repository = repository;
    }
    public async Task<LotDto> Handle(DeleteLotCommand request, CancellationToken cancellationToken)
    {
        var lot = await _repository.GetById<Lot>(request.Id)
            ?? throw new ArgumentNullException("Lot cannot be found");

        if (lot.Auction?.StatusId != (int)AuctionStatusId.Created)
        {
            throw new ArgumentException("Cannot edit lot of started auction");
        }

        lot = await _repository.Remove<Lot>(request.Id);

        await _repository.SaveChanges();

        var lotDto = LotDto.FromLot(lot);

        return lotDto;
    }
}
