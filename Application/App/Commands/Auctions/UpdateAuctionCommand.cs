using Application.App.Responses;
using MediatR;

namespace Application.App.Commands.Auctions;
public class UpdateAuctionCommand : IRequest<AuctionDto>
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public DateTimeOffset? StartTime { get; set; }

    public DateTimeOffset? EndTime { get; set; }

    public List<int>? LotIds { get; set; }
}
