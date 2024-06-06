using AuctionApp.Domain.Abstractions;
using Domain.Auth;

namespace AuctionApp.Domain.Models
{
    public class ProductReview : IEntity
    {
        public int Id { get; set; }

        public int? UserId { get; set; }

        public User? User { get; set; }

        public int ProductId { get; set; }

        public Product? Product { get; set; }

        public string? ReviewText { get; set; }

        public float Rating { get; set; }
    }
}
