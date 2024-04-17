using Application.App.Responses;
using MediatR;

namespace Application.App.Commands.Bids;
public class CreateBidCommand : IRequest<BidDto>
{
    public required int LotId { get; set; }

    public required decimal Amount { get; set; }
}
