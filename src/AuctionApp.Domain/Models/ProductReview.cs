using Domain.Auth;
using EntityFramework.Domain.Abstractions;

namespace AuctionApp.Domain.Models
{
    public class ProductReview : Entity
    {
        public int? UserId { get; set; }

        public User? User { get; set; }

        public int ProductId { get; set; }

        public Product? Product { get; set; }

        public string? ReviewText { get; set; }

        public float Rating { get; set; }
    }
}
