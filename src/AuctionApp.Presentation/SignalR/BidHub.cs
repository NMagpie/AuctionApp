namespace AuctionApp.Presentation.SignalR;

using Application.App.Bids.Commands;
using Application.App.Queries;
using AuctionApp.Presentation.SignalR.Dtos;
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

        var result = await _mediator.Send(command);

        await Clients.Group(command.ProductId.ToString()).SendAsync("BidNotify", result);
    }

    public async Task GetLatestPrice(int productId)
    {
        var command = new GetLatestProductPriceByIdQuery { Id = productId };

        var result = await _mediator.Send(command);

        await Clients.Caller.SendAsync("GetLatestPrice", result);
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
