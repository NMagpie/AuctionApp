namespace AuctionApp.Presentation.SignalR.Dtos;
public class CreateBidRequest
{
    public required int ProductId { get; set; }

    public required decimal Amount { get; set; }
}
