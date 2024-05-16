namespace Application.App.Bids.Responses;
public class BidDto
{
    public int Id { get; set; }

    public required int LotId { get; set; }

    public int UserId { get; set; }

    public required decimal Amount { get; set; }

    public required DateTimeOffset CreateTime { get; set; }

    public required bool IsWon { get; set; }
}
