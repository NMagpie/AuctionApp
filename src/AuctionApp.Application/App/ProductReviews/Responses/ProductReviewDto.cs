namespace Application.App.ProductReviews.Responses;
public class ProductReviewDto
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public required int ProductId { get; set; }

    public string? ReviewText { get; set; }

    public required float Rating { get; set; }
}
