namespace Presentation.Common.Models.Auctions;
public class UpdateAuctionRequest
{
    public string Title { get; set; }

    public DateTimeOffset StartTime { get; set; }

    public DateTimeOffset EndTime { get; set; }
}
