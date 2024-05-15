namespace Presentation.Common.Requests.AuctionReviews;
public class CreateAuctionReviewRequest
{
    public int AuctionId { get; set; }

    public string? ReviewText { get; set; }

    public float Rating { get; set; }
}
