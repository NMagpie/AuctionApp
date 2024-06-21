using AuctionApp.Domain.Abstractions;
using Domain.Auth;
using EntityFramework.Domain.Models;

namespace AuctionApp.Domain.Models;
public class Product : IEntity
{
    public int Id { get; set; }

    public string Title { get; set; }

    public string? Description { get; set; }

    public int? CreatorId { get; set; }

    public User? Creator { get; set; }

    public DateTimeOffset? StartTime { get; set; }

    public DateTimeOffset? EndTime { get; set; }

    public bool SellingFinished { get; set; }

    public decimal InitialPrice { get; set; }

    public ICollection<Bid>? Bids { get; set; } = [];

    public int? CategoryId { get; set; }

    public Category? Category { get; set; }
}
