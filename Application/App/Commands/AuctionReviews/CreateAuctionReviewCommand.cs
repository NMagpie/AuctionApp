using Application.App.Responses;
using MediatR;

namespace Application.App.Commands.AuctionReviews;
public class CreateAuctionReviewCommand : IRequest<AuctionReviewDto>
{
    public int UserId { get; set; }

    public int AuctionId { get; set; }

    public string? ReviewText { get; set; }

    public float Rating { get; set; }
}
