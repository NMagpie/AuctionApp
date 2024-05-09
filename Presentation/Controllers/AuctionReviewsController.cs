using Application.App.AuctionReviews.Commands;
using Application.App.AuctionReviews.Responses;
using Application.App.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[ApiController]
[Route("[controller]")]
public class AuctionReviewsController : ControllerBase
{

    private readonly IMediator _mediator;

    public AuctionReviewsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AuctionReviewDto>> GetAuctionReview(int id)
    {
        var auctionReviewDto = await _mediator.Send(new GetAuctionReviewByIdQuery() { Id = id });

        return Ok(auctionReviewDto);
    }

    [HttpPost]
    public async Task<ActionResult<AuctionReviewDto>> CreateAuctionReview(CreateAuctionReviewCommand createAuctionReviewCommand)
    {
        var auctionReviewDto = await _mediator.Send(createAuctionReviewCommand);

        return Ok(auctionReviewDto);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<AuctionReviewDto>> UpdateAuctionReview(UpdateAuctionReviewCommand updateAuctionReviewCommand)
    {
        var auctionReviewDto = await _mediator.Send(updateAuctionReviewCommand);

        return Ok(auctionReviewDto);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAuctionReview(int id)
    {
        await _mediator.Send(new DeleteAuctionReviewCommand() { Id = id });

        return Ok();
    }
}