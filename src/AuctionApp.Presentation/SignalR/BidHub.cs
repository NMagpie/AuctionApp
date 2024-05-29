namespace AuctionApp.Presentation.SignalR;

using Application.App.Bids.Commands;
using AuctionApp.Presentation.Common.Requests.Bids;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using System.Threading.Tasks;

[Authorize]
public class BidHub : Hub
{
    private readonly IMediator _mediator;

    private readonly IMapper _mapper;

    public BidHub(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    public async Task PutBid(CreateBidRequest request)
    {
        var userName = Context.User?.FindFirst(ClaimTypes.Name)?.Value;

        var userId = int.Parse(Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

        var command = _mapper.Map<CreateBidRequest, CreateBidCommand>(request);

        command.UserId = userId;

        await _mediator.Send(command);

        await Clients.Group(command.ProductId.ToString()).SendAsync("BidNotify", $"{userName} puts bid: {command.Amount}");
    }

    public async Task AddToProductGroup(string productId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, productId);
    }

    public async Task RemoveFromProductGroup(string productId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, productId);
    }
}
