using EntityFramework.Domain.Abstractions;

namespace AuctionApp.Domain.Models
{
    public class UserWatchlist : Entity
    {
        public required int UserId { get; set; }

        public User User { get; set; }

        public required int AuctionId { get; set; }

        public Auction Auction { get; set; }
    }
}
