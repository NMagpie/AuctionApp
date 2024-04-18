using Application.App.Responses;
using MediatR;

namespace Application.App.Commands.Lots;
public class UpdateLotCommand : IRequest<LotDto>
{
    public int Id { get; set; }
    public string? Title { get; set; }

    public string? Description { get; set; }

    public int AuctionId { get; set; }

    public decimal InitialPrice { get; set; }

    public HashSet<CategoryDto> Categories { get; set; } = [];
}
