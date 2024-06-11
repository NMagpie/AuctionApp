
using Application.App.Bids.Commands;
using Application.App.Bids.Responses;
using Application.App.Queries;
using AuctionApp.Presentation.SignalR.Dtos;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace AuctionApp.Presentation.SignalR;
public class BidsHub : Hub
{
    private readonly IMediator _mediator;

    private readonly IMapper _mapper;

    public BidsHub(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [Authorize]
    public async Task PlaceBid(CreateBidRequest request)
    {
        var userName = Context.User?.FindFirst(ClaimTypes.Name)?.Value;

        var userId = int.Parse(Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

        var command = _mapper.Map<CreateBidRequest, CreateBidCommand>(request);

        command.UserId = userId;

        var result = await _mediator.Send(command);

        var response = _mapper.Map<BidDto, CreateBidResponse>(result);

        response.UserName = userName!;

        await Clients.Group(command.ProductId.ToString()).SendAsync("BidNotify", response);
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
