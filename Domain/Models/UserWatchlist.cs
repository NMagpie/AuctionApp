using EntityFramework.Domain.Abstractions;

namespace AuctionApp.Domain.Models
{
    public class UserWatchlist : Entity
    {
        public int UserId { get; set; }

        public User? User { get; set; }

        public int AuctionId { get; set; }

        public Auction? Auction { get; set; }
    }
}
