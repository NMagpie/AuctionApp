using Application.Abstractions;
using Application.App.UserWatchlists.Responses;
using AuctionApp.Domain.Models;
using MediatR;

namespace Application.App.Queries;
public class GetUserWatchlistByIdQuery : IRequest<UserWatchlistDto>
{
    public int Id { get; set; }
}

public class GetUserWatchlistByIdQueryHandler : IRequestHandler<GetUserWatchlistByIdQuery, UserWatchlistDto>
{
    private readonly IRepository _repository;

    public GetUserWatchlistByIdQueryHandler(IRepository repository)
    {
        _repository = repository;
    }

    public async Task<UserWatchlistDto> Handle(GetUserWatchlistByIdQuery request, CancellationToken cancellationToken)
    {
        var auction = await _repository.GetById<UserWatchlist>(request.Id)
            ?? throw new ArgumentNullException("User watchlist cannot be found");

        var auctionDto = UserWatchlistDto.FromUserWatchlist(auction);

        return auctionDto;
    }
}