using Application.App.Lots.Responses;
using Application.Common.Abstractions;
using Application.Common.Exceptions;
using AuctionApp.Domain.Models;
using AutoMapper;
using MediatR;

namespace Application.App.Queries;
public class GetLotByIdQuery : IRequest<LotDto>
{
    public int Id { get; set; }
}

public class GetLotByIdQueryHandler : IRequestHandler<GetLotByIdQuery, LotDto>
{
    private readonly IRepository _repository;

    private readonly IMapper _mapper;

    public GetLotByIdQueryHandler(IRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<LotDto> Handle(GetLotByIdQuery request, CancellationToken cancellationToken)
    {
        var lot = await _repository.GetById<Lot>(request.Id)
            ?? throw new EntityNotFoundException("Lot cannot be found");

        var lotDto = _mapper.Map<Lot, LotDto>(lot);

        return lotDto;
    }
}
