namespace AuctionApp.Presentation.SignalR.Dtos;
public class CreateBidResponse
{
    public int Id { get; set; }

    public required int ProductId { get; set; }

    public int UserId { get; set; }

    public required string UserName { get; set; }

    public required decimal Amount { get; set; }

    public required DateTimeOffset CreateTime { get; set; }
}

