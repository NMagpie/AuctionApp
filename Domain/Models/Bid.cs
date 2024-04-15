using EntityFramework.Domain.Abstractions;

namespace AuctionApp.Domain.Models;
public class Bid : Entity
{
    public int? LotId { get; set; }

    public Lot? Lot { get; set; }

    public required decimal Amount { get; set; }

    public required DateTimeOffset CreateTime { get; set; }

    public required bool IsWon { get; set; }
}