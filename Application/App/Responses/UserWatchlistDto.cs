using AuctionApp.Domain.Models;

namespace Application.App.Responses;
public class UserWatchlistDto
{
    public int Id { get; set; }

    public required int UserId { get; set; }

    public int? AuctionId { get; set; }

    public static UserWatchlistDto FromUserWatchlist(UserWatchlist userWatchlist)
    {
        return new UserWatchlistDto
        {
            Id = userWatchlist.Id,
            UserId = userWatchlist.UserId,
            AuctionId = userWatchlist.AuctionId,
        };
    }
}
