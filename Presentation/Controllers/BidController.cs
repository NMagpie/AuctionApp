using Application.App.Bids.Commands;
using Application.App.Bids.Responses;
using Application.App.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[ApiController]
[Route("[controller]")]
public class BidController : ControllerBase
{
    private readonly ILogger<BidController> _logger;

    private readonly IMediator _mediator;

    public BidController(ILogger<BidController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpGet("{id}")]
    public async Task<BidDto> GetBid(int id)
    {
        var bidDto = await _mediator.Send(new GetBidByIdQuery() { Id = id });

        return bidDto;
    }

    [HttpPost]
    public async Task<BidDto> CreateBid(CreateBidCommand createBidCommand)
    {
        var bidDto = await _mediator.Send(createBidCommand);

        return bidDto;
    }

    [HttpDelete("{id}")]
    public async Task<BidDto> DeleteBid(int id)
    {
        var bidDto = await _mediator.Send(new DeleteBidCommand() { Id = id });

        return bidDto;
    }
}