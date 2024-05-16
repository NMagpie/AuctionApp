using Application.App.Lots.Responses;
using Application.Common.Abstractions;
using Application.Common.Exceptions;
using AuctionApp.Domain.Models;
using AutoMapper;
using MediatR;

namespace Application.App.Lots.Commands;

public class UpdateLotCommand : IRequest<LotDto>
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public decimal InitialPrice { get; set; }

    public HashSet<string> Categories { get; set; } = [];
}

public class UpdateLotCommandHandler : IRequestHandler<UpdateLotCommand, LotDto>
{

    private readonly IEntityRepository _repository;

    private readonly IMapper _mapper;

    public UpdateLotCommandHandler(IEntityRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<LotDto> Handle(UpdateLotCommand request, CancellationToken cancellationToken)
    {
        var lot = await _repository.GetByIdWithInclude<Lot>(request.Id, lot => lot.Auction)
            ?? throw new EntityNotFoundException("Lot cannot be found");

        if (lot.Auction.CreatorId != request.UserId)
        {
            throw new InvalidUserException("You do not have permission to modify this data");
        }

        if (lot.Auction.StartTime <= DateTime.UtcNow + TimeSpan.FromMinutes(5))
        {
            throw new BusinessValidationException("Cannot edit lots of auction 5 minutes before its start");
        }

        _mapper.Map(request, lot);

        await _repository.SaveChanges();

        var lotDto = _mapper.Map<Lot, LotDto>(lot);

        return lotDto;
    }
}
