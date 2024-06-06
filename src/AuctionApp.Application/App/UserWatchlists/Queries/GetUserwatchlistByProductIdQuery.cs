using Application.App.UserWatchlists.Responses;
using Application.Common.Abstractions;
using Application.Common.Exceptions;
using AuctionApp.Domain.Models;
using AutoMapper;
using MediatR;

namespace Application.App.Queries;
public class GetUserwatchlistByProductIdQuery : IRequest<UserWatchlistDto>
{
    public int ProductId { get; set; }

    public int UserId { get; set; }
}

public class GetUserwatchlistByProductIdQueryHandler : IRequestHandler<GetUserwatchlistByProductIdQuery, UserWatchlistDto>
{
    private readonly IEntityRepository _repository;

    private readonly IMapper _mapper;

    public GetUserwatchlistByProductIdQueryHandler(IEntityRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<UserWatchlistDto> Handle(GetUserwatchlistByProductIdQuery request, CancellationToken cancellationToken)
    {

        var userWatchlist = (await _repository.GetByPredicate<UserWatchlist>(uw => uw.ProductId == request.ProductId && uw.UserId == request.UserId)).FirstOrDefault()
            ?? throw new EntityNotFoundException("User watchlist cannot be found");

        var userWatchlistDto = _mapper.Map<UserWatchlist, UserWatchlistDto>(userWatchlist);

        return userWatchlistDto;
    }
}