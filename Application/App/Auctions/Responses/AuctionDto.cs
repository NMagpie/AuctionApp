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
}