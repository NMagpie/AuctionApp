using Application.App.Auctions.Commands;
using Application.App.Auctions.Queries;
using Application.App.Auctions.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[ApiController]
[Route("[controller]")]
public class AuctionsController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuctionsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id}")]
    public async Task<AuctionDto> GetAuction(int id)
    {
        var auctionDto = await _mediator.Send(new GetAuctionByIdQuery() { Id = id });

        return auctionDto;
    }

    [HttpPost]
    public async Task<AuctionDto> CreateAuction(CreateAuctionCommand createAuctionCommand)
    {
        var auctionDto = await _mediator.Send(createAuctionCommand);

        return auctionDto;
    }

    [HttpPut("{id}")]
    public async Task<AuctionDto> UpdateAuction(UpdateAuctionCommand updateAuctionCommand)
    {
        var auctionDto = await _mediator.Send(updateAuctionCommand);

        return auctionDto;
    }

    [HttpDelete("{id}")]
    public async Task DeleteAuction(int id)
    {
        await _mediator.Send(new DeleteAuctionCommand() { Id = id });
    }

}

