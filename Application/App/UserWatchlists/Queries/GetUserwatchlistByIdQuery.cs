using Application.App.UserWatchlists.Responses;
using Application.Common.Abstractions;
using Application.Common.Exceptions;
using AuctionApp.Domain.Models;
using AutoMapper;
using MediatR;

namespace Application.App.Queries;
public class GetUserWatchlistByIdQuery : IRequest<UserWatchlistDto>
{
    public int Id { get; set; }
}

public class GetUserWatchlistByIdQueryHandler : IRequestHandler<GetUserWatchlistByIdQuery, UserWatchlistDto>
{
    private readonly IRepository _repository;

    private readonly IMapper _mapper;

    public GetUserWatchlistByIdQueryHandler(IRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<UserWatchlistDto> Handle(GetUserWatchlistByIdQuery request, CancellationToken cancellationToken)
    {
        var auction = await _repository.GetById<UserWatchlist>(request.Id)
            ?? throw new EntityNotFoundException("User watchlist cannot be found");

        var auctionDto = _mapper.Map<UserWatchlist, UserWatchlistDto>(auction);

        return auctionDto;
    }
}