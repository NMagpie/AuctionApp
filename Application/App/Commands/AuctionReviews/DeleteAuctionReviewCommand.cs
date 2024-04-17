using Application.App.Responses;
using MediatR;

namespace Application.App.Commands.AuctionReviews;
public class DeleteAuctionReviewCommand : IRequest<AuctionReviewDto>
{
    public int Id { get; set; }
}
