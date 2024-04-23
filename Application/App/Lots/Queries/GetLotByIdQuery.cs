using Application.Abstractions;
using Application.App.Lots.Responses;
using AuctionApp.Domain.Models;
using MediatR;

namespace Application.App.Queries;
public class GetLotByIdQuery : IRequest<LotDto>
{
    public int Id { get; set; }
}

public class GetLotByIdQueryHandler : IRequestHandler<GetLotByIdQuery, LotDto>
{
    private readonly IRepository _repository;

    public GetLotByIdQueryHandler(IRepository repository)
    {
        _repository = repository;
    }

    public async Task<LotDto> Handle(GetLotByIdQuery request, CancellationToken cancellationToken)
    {
        var auction = await _repository.GetById<Lot>(request.Id)
            ?? throw new ArgumentNullException("Lot cannot be found");

        var auctionDto = LotDto.FromLot(auction);

        return auctionDto;
    }
}
