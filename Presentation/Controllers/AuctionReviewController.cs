using Application.App.AuctionReviews.Commands;
using Application.App.AuctionReviews.Responses;
using Application.App.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[ApiController]
[Route("[controller]")]
public class AuctionReviewController : ControllerBase
{
    private readonly ILogger<AuctionReviewController> _logger;

    private readonly IMediator _mediator;

    public AuctionReviewController(ILogger<AuctionReviewController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpGet("{id}")]
    public async Task<AuctionReviewDto> GetAuctionReview(int id)
    {
        var auctionReviewDto = await _mediator.Send(new GetAuctionReviewByIdQuery() { Id = id });

        return auctionReviewDto;
    }

    [HttpPost]
    public async Task<AuctionReviewDto> CreateAuctionReview(CreateAuctionReviewCommand createAuctionReviewCommand)
    {
        var auctionReviewDto = await _mediator.Send(createAuctionReviewCommand);

        return auctionReviewDto;
    }

    [HttpPut]
    public async Task<AuctionReviewDto> UpdateAuctionReview(UpdateAuctionReviewCommand updateAuctionReviewCommand)
    {
        var auctionReviewDto = await _mediator.Send(updateAuctionReviewCommand);

        return auctionReviewDto;
    }

    [HttpDelete("{id}")]
    public async Task<AuctionReviewDto> DeleteAuctionReview(int id)
    {
        var auctionReviewDto = await _mediator.Send(new DeleteAuctionReviewCommand() { Id = id });

        return auctionReviewDto;
    }
}