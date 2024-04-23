using AuctionApp.Domain.Models;

namespace Application.App.Bids.Responses;
public class BidDto
{
    public int Id { get; set; }

    public required int LotId { get; set; }

    public required decimal Amount { get; set; }

    public required DateTimeOffset CreateTime { get; set; }

    public required bool IsWon { get; set; }

    public static BidDto FromBid(Bid bid)
    {
        return new BidDto
        {
            Id = bid.Id,
            LotId = bid.LotId,
            Amount = bid.Amount,
            CreateTime = bid.CreateTime,
            IsWon = bid.IsWon,
        };
    }
}
