using Application.App.Responses;

namespace Application.App.Lots.Responses;
public class LotDto
{
    public int Id { get; set; }

    public required string Title { get; set; }

    public string? Description { get; set; }

    public int? AuctionId { get; set; }

    public decimal? InitialPrice { get; set; }

    public HashSet<int> BidIds { get; set; } = [];

    public HashSet<CategoryDto> Categories { get; set; } = [];
}
