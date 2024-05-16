using Application.App.Auctions.Commands;
using Application.App.Auctions.Queries;
using Application.App.Auctions.Responses;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Common.Abstractions;
using Presentation.Common.Models.Auctions;
using Presentation.Common.Requests.Auctions;

namespace Presentation.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class AuctionsController : AppBaseController
{
    private readonly IMediator _mediator;

    private readonly IMapper _mapper;

    public AuctionsController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<ActionResult<AuctionDto>> GetAuction(int id)
    {
        var auctionDto = await _mediator.Send(new GetAuctionByIdQuery() { Id = id });

        return Ok(auctionDto);
    }

    [HttpPost]
    public async Task<ActionResult<AuctionDto>> CreateAuction(CreateAuctionRequest createAuctionRequest)
    {
        var userId = GetUserId();

        var auctionCommand = _mapper.Map<CreateAuctionRequest, CreateAuctionCommand>(createAuctionRequest);

        auctionCommand.CreatorId = userId;

        var auctionDto = await _mediator.Send(auctionCommand);

        return Ok(auctionDto);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<AuctionDto>> UpdateAuction(int id, UpdateAuctionRequest updateAuctionRequest)
    {
        var userId = GetUserId();

        var auctionCommand = _mapper.Map<UpdateAuctionRequest, UpdateAuctionCommand>(updateAuctionRequest);

        auctionCommand.Id = id;

        auctionCommand.CreatorId = userId;

        var auctionDto = await _mediator.Send(auctionCommand);

        return Ok(auctionDto);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAuction(int id)
    {
        var userId = GetUserId();

        await _mediator.Send(new DeleteAuctionCommand() { Id = id, CreatorId = userId });

        return Ok();
    }

}

