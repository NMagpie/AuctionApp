using Application.App.Responses;
using MediatR;

namespace Application.App.Commands.UserWatchlist;
public class CreateUserWatchlistCommand : IRequest<UserWatchlistDto>
{
    public int UserId { get; set; }

    public int AuctionId { get; set; }
}

