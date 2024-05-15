using Application.App.AuctionReviews.Commands;
using Application.App.AuctionReviews.Responses;
using Application.App.Queries;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Common.Abstractions;
using Presentation.Common.Models.AuctionReviews;
using Presentation.Common.Requests.AuctionReviews;

namespace Presentation.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class AuctionReviewsController : AppBaseController
{
    private readonly IMediator _mediator;

    private readonly IMapper _mapper;

    public AuctionReviewsController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<ActionResult<AuctionReviewDto>> GetAuctionReview(int id)
    {
        var auctionReviewDto = await _mediator.Send(new GetAuctionReviewByIdQuery() { Id = id });

        return Ok(auctionReviewDto);
    }

    [HttpPost]
    public async Task<ActionResult<AuctionReviewDto>> CreateAuctionReview(CreateAuctionReviewRequest createAuctionReviewRequest)
    {
        var userId = GetUserId();

        var auctionReviewCommand = _mapper.Map<CreateAuctionReviewRequest, CreateAuctionReviewCommand>(createAuctionReviewRequest);

        auctionReviewCommand.UserId = userId;

        var auctionReviewDto = await _mediator.Send(auctionReviewCommand);

        return Ok(auctionReviewDto);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<AuctionReviewDto>> UpdateAuctionReview(int id, UpdateAuctionReviewRequest updateAuctionReviewRequest)
    {
        var userId = GetUserId();

        var auctionReviewCommand = _mapper.Map<UpdateAuctionReviewRequest, UpdateAuctionReviewCommand>(updateAuctionReviewRequest);

        auctionReviewCommand.Id = id;

        auctionReviewCommand.UserId = userId;

        var auctionReviewDto = await _mediator.Send(auctionReviewCommand);

        return Ok(auctionReviewDto);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAuctionReview(int id)
    {
        var userId = GetUserId();

        await _mediator.Send(new DeleteAuctionReviewCommand() { Id = id, UserId = userId });

        return Ok();
    }
}