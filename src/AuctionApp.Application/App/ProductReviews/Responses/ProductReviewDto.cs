using Application.App.Users.Responses;

namespace Application.App.ProductReviews.Responses;
public class ProductReviewDto
{
    public int Id { get; set; }

    public UserDto User { get; set; }

    public required int ProductId { get; set; }

    public string? ReviewText { get; set; }

    public required float Rating { get; set; }

    public required DateTimeOffset DateCreated { get; set; }
}
