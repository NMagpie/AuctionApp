namespace AuctionApp.Presentation.Common.Requests.Bids;
public class CreateBidRequest
{
    public required int ProductId { get; set; }

    public required decimal Amount { get; set; }
}
