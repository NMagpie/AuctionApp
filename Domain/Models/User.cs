using EntityFramework.Domain.Abstractions;

namespace AuctionApp.Domain.Models
{
    public class User : Entity
    {
        public required string Username { get; set; }

        public decimal Balance { get; set; }
    }
}
