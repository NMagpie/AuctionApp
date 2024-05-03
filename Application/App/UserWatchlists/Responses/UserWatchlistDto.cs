using AuctionApp.Domain.Models;

namespace Application.App.UserWatchlists.Responses;
public class UserWatchlistDto
{
    public int Id { get; set; }

    public required int UserId { get; set; }

    public int? AuctionId { get; set; }
}
