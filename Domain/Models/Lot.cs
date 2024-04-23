using EntityFramework.Domain.Abstractions;
using EntityFramework.Domain.Models;

namespace AuctionApp.Domain.Models;
public class Lot : Entity
{
    public required string Title { get; set; }

    public string? Description { get; set; }

    public required int AuctionId { get; set; }

    public Auction Auction { get; set; }

    public decimal InitialPrice { get; set; }

    public ICollection<Bid>? Bids { get; set; } = [];

    public ICollection<Category>? Categories { get; set; } = [];
}
