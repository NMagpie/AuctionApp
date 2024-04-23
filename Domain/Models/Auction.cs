using EntityFramework.Domain.Abstractions;
using EntityFramework.Domain.Models;

namespace AuctionApp.Domain.Models;
public class Auction : Entity
{
    public required string Title { get; set; }

    public int? CreatorId { get; set; }

    public User? Creator { get; set; }

    public DateTimeOffset? StartTime { get; set; }

    public DateTimeOffset? EndTime { get; set; }

    public required int StatusId { get; set; }

    public AuctionStatus? Status { get; set; }

    public ICollection<Lot>? Lots { get; set; } = [];
}
