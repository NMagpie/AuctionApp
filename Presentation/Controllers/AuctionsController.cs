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
    public async Task<ActionResult<AuctionDto>> GetAuction(int id)
    {
        var auctionDto = await _mediator.Send(new GetAuctionByIdQuery() { Id = id });

        return Ok(auctionDto);
    }

    [HttpPost]
    public async Task<ActionResult<AuctionDto>> CreateAuction(CreateAuctionCommand createAuctionCommand)
    {
        var auctionDto = await _mediator.Send(createAuctionCommand);

        return Ok(auctionDto);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<AuctionDto>> UpdateAuction(UpdateAuctionCommand updateAuctionCommand)
    {
        var auctionDto = await _mediator.Send(updateAuctionCommand);

        return Ok(auctionDto);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAuction(int id)
    {
        await _mediator.Send(new DeleteAuctionCommand() { Id = id });

        return Ok();
    }

}

