using AuctionApp.Domain.Abstractions;
using Domain.Auth;

namespace AuctionApp.Domain.Models
{
    public class UserWatchlist : IEntity
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public User? User { get; set; }

        public int ProductId { get; set; }

        public Product? Product { get; set; }

        public DateTimeOffset Created { get; set; }
    }
}
