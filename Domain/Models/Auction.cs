using Domain.Auth;
using EntityFramework.Domain.Abstractions;

namespace AuctionApp.Domain.Models;
public class Auction : Entity
{
    public string Title { get; set; }

    public int? CreatorId { get; set; }

    public User? Creator { get; set; }

    public DateTimeOffset? StartTime { get; set; }

    public DateTimeOffset? EndTime { get; set; }

    public ICollection<Lot>? Lots { get; set; } = [];
}
