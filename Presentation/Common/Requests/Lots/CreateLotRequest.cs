namespace Presentation.Common.Requests.Lots;
public class CreateLotRequest
{
    public string Title { get; set; }

    public string? Description { get; set; }

    public int AuctionId { get; set; }

    public decimal InitialPrice { get; set; }

    public HashSet<string> Categories { get; set; } = [];
}