namespace Presentation.Common.Requests.ProductReviews;
public class CreateProductReviewRequest
{
    public int ProductId { get; set; }

    public string? ReviewText { get; set; }

    public float Rating { get; set; }
}
