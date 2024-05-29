using Domain.Auth;
using EntityFramework.Domain.Abstractions;
using EntityFramework.Domain.Models;

namespace AuctionApp.Domain.Models;
public class Product : Entity
{
    public string Title { get; set; }

    public string? Description { get; set; }

    public int? CreatorId { get; set; }

    public User? Creator { get; set; }

    public DateTimeOffset? StartTime { get; set; }

    public DateTimeOffset? EndTime { get; set; }

    public decimal InitialPrice { get; set; }

    public ICollection<Bid>? Bids { get; set; } = [];

    public ICollection<Category>? Categories { get; set; } = [];
}
