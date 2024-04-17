using Application.App.Responses;
using MediatR;

namespace Application.App.Commands.AuctionReviews;
public class UpdateAuctionReviewCommand : IRequest<AuctionReviewDto>
{
    public int Id { get; set; }

    public string? ReviewText { get; set; }

    public required float Rating { get; set; }
}
