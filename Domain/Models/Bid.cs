using EntityFramework.Domain.Abstractions;

namespace AuctionApp.Domain.Models;
public class Bid : Entity
{
    public required int LotId { get; set; }

    public required Lot Lot { get; set; }

    public required decimal Amount { get; set; }

    public required DateTimeOffset CreateTime { get; set; }

    public required bool IsWon { get; set; }
}