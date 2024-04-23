using AuctionApp.Domain.Models;

namespace Application.App.AuctionReviews.Responses;
public class AuctionReviewDto
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public required int AuctionId { get; set; }

    public string? ReviewText { get; set; }

    public required float Rating { get; set; }

    public static AuctionReviewDto FromAuctionReview(AuctionReview auctionReview)
    {
        return new AuctionReviewDto
        {
            Id = auctionReview.Id,
            UserId = auctionReview.UserId,
            AuctionId = auctionReview.AuctionId,
            ReviewText = auctionReview.ReviewText,
            Rating = auctionReview.Rating,
        };
    }
}
