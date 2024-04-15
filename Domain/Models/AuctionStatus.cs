using EntityFramework.Domain.Abstractions;

namespace EntityFramework.Domain.Models;

public class AuctionStatus : Entity
{
    public required string Status { get; set; }
}
