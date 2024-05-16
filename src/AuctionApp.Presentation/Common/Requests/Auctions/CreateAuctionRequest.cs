using Application.Common.Models;

namespace Presentation.Common.Requests.Auctions;
public class CreateAuctionRequest
{
    public string Title { get; set; }

    public DateTimeOffset StartTime { get; set; }

    public DateTimeOffset EndTime { get; set; }

    public List<LotInAuctionDto> Lots { get; set; } = [];
}
