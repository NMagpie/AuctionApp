using Application.App.Bids.Responses;
using Application.Common.Abstractions;
using Application.Common.Exceptions;
using AuctionApp.Domain.Models;
using AutoMapper;
using MediatR;

namespace Application.App.Queries;
public class GetBidByIdQuery : IRequest<BidDto>
{
    public int Id { get; set; }
}

public class GetBidByIdQueryHandler : IRequestHandler<GetBidByIdQuery, BidDto>
{
    private readonly IRepository _repository;

    private readonly IMapper _mapper;

    public GetBidByIdQueryHandler(IRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<BidDto> Handle(GetBidByIdQuery request, CancellationToken cancellationToken)
    {
        var bid = await _repository.GetById<Bid>(request.Id)
            ?? throw new EntityNotFoundException("Bid cannot be found");

        var bidDto = _mapper.Map<Bid, BidDto>(bid);

        return bidDto;
    }
}
