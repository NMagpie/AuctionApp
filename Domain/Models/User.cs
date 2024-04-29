using EntityFramework.Domain.Abstractions;

namespace AuctionApp.Domain.Models
{
    public class User : Entity
    {
        public string Username { get; set; }

        public decimal Balance { get; set; }
    }
}
