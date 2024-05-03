using AuctionApp.Domain.Models;

namespace Application.App.AuctionReviews.Responses;
public class AuctionReviewDto
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public required int AuctionId { get; set; }

    public string? ReviewText { get; set; }

    public required float Rating { get; set; }
}
