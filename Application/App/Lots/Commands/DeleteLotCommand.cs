using Application.Abstractions;
using Application.App.Lots.Responses;
using AuctionApp.Domain.Models;
using AutoMapper;
using MediatR;

namespace Application.App.Lots.Commands;

public class DeleteLotCommand : IRequest<LotDto>
{
    public int Id { get; set; }
}

public class DeleteLotCommandHandler : IRequestHandler<DeleteLotCommand, LotDto>
{
    private readonly IRepository _repository;

    private readonly IMapper _mapper;

    public DeleteLotCommandHandler(IRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    public async Task<LotDto> Handle(DeleteLotCommand request, CancellationToken cancellationToken)
    {
        var lot = await _repository.GetByIdWithInclude<Lot>(request.Id, lot => lot.Auction)
            ?? throw new ArgumentNullException("Lot cannot be found");

        if (lot.Auction.StartTime <= DateTime.UtcNow + TimeSpan.FromMinutes(5))
        {
            throw new ArgumentException("Cannot edit lots of auction 5 minutes before its start");
        }

        if (lot.Auction.Lots.Count <= 1)
        {
            throw new ArgumentException("Cannot delete lot: must be at least one lot present");
        }

        lot = await _repository.Remove<Lot>(request.Id);

        await _repository.SaveChanges();

        var lotDto = _mapper.Map<Lot, LotDto>(lot);

        return lotDto;
    }
}
