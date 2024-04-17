using AuctionApp.Domain.Models;

namespace Application.App.Responses;
public class LotDto
{
    public int Id { get; set; }

    public required string Title { get; set; }

    public string? Description { get; set; }

    public int? AuctionId { get; set; }

    public decimal? InitialPrice { get; set; }

    public HashSet<int> BidIds { get; set; } = [];

    public HashSet<CategoryDto> Categories { get; set; } = [];

    public static LotDto FromLot(Lot lot)
    {
        return new LotDto
        {
            Id = lot.Id,
            Title = lot.Title,
            Description = lot.Description,
            AuctionId = lot.AuctionId,
            InitialPrice = lot.InitialPrice,
            BidIds = lot.Bids?.Select(bid => bid.Id).ToHashSet() ?? [],
            Categories = lot.Categories?.Select(category => new CategoryDto { Id = category.Id, Name = category.Name }).ToHashSet() ?? [],
        };
    }
}
