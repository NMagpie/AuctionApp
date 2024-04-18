using Application.App.Responses;
using MediatR;

namespace Application.App.Commands.Auctions;
public class CreateAuctionCommand : IRequest<AuctionDto>
{
    public required string Title { get; set; }

    public int CreatorId { get; set; }

    public DateTimeOffset? StartTime { get; set; }

    public DateTimeOffset? EndTime { get; set; }

    public HashSet<int> LotIds { get; set; } = [];
}
