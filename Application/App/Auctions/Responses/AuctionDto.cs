using AuctionApp.Domain.Models;

namespace Application.App.Auctions.Responses;
public class AuctionDto
{
    public int Id { get; set; }

    public required string Title { get; set; }

    public int? CreatorId { get; set; }

    public DateTimeOffset? StartTime { get; set; }

    public DateTimeOffset? EndTime { get; set; }

    public HashSet<int> LotIds { get; set; } = [];

    public static AuctionDto FromAuction(Auction auction)
    {
        return new AuctionDto
        {
            Id = auction.Id,
            Title = auction.Title,
            CreatorId = auction.CreatorId,
            StartTime = auction.StartTime,
            EndTime = auction.EndTime,
            LotIds = auction.Lots?.Select(lot => lot.Id).ToHashSet() ?? []
        };
    }
}