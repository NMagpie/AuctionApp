namespace Application.Models;
public class LotInAuctionDto
{
    public string Title { get; set; }

    public string? Description { get; set; }

    public decimal InitialPrice { get; set; }

    public HashSet<CategoryInLotDto> Categories { get; set; } = [];
}
