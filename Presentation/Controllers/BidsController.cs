using Application.App.Bids.Commands;
using Application.App.Bids.Responses;
using Application.App.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[ApiController]
[Route("[controller]")]
public class BidsController : ControllerBase
{
    private readonly IMediator _mediator;

    public BidsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BidDto>> GetBid(int id)
    {
        var bidDto = await _mediator.Send(new GetBidByIdQuery() { Id = id });

        return Ok(bidDto);
    }

    [HttpPost]
    public async Task<ActionResult<BidDto>> CreateBid(CreateBidCommand createBidCommand)
    {
        var bidDto = await _mediator.Send(createBidCommand);

        return Ok(bidDto);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteBid(int id)
    {
        await _mediator.Send(new DeleteBidCommand() { Id = id });

        return Ok();
    }
}