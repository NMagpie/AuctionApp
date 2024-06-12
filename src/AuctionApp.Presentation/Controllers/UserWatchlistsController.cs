using Application.App.Queries;
using Application.App.UserWatchlists.Commands;
using Application.App.UserWatchlists.Responses;
using AuctionApp.Application.App.UserWatchlists.Commands;
using AuctionApp.Presentation.Common.Requests.UserWatchlists;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Common.Abstractions;
using Swashbuckle.AspNetCore.Annotations;

namespace Presentation.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class UserWatchlistsController : AppBaseController
{
    private readonly IMediator _mediator;

    public UserWatchlistsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id}")]
    [SwaggerOperation(OperationId = nameof(GetUserWatchlist))]
    public async Task<ActionResult<UserWatchlistDto>> GetUserWatchlist(int id)
    {
        var userId = GetUserId();

        var userWatchlistDto = await _mediator.Send(new GetUserWatchlistByIdQuery() { Id = id, UserId = userId });

        return Ok(userWatchlistDto);
    }

    [HttpHead]
    [SwaggerOperation(OperationId = nameof(ExistsUserWatchlistByProductId))]
    public async Task<ActionResult<UserWatchlistDto>> ExistsUserWatchlistByProductId([FromQuery] int productId)
    {
        var userId = GetUserId();

        await _mediator.Send(new GetUserwatchlistByProductIdQuery() { ProductId = productId, UserId = userId });

        return Ok();
    }

    [HttpPost]
    [SwaggerOperation(OperationId = nameof(CreateUserWatchlist))]
    public async Task<ActionResult<UserWatchlistDto>> CreateUserWatchlist(CreateUserWatchlistRequest createUserWatchlistRequest)
    {
        var userId = GetUserId();

        var command = new CreateUserWatchlistCommand
        {
            UserId = userId,
            ProductId = createUserWatchlistRequest.ProductId,
        };

        var userWatchlistDto = await _mediator.Send(command);

        return Ok(userWatchlistDto);
    }

    [HttpDelete("{id}")]
    [SwaggerOperation(OperationId = nameof(DeleteUserWatchlist))]
    public async Task<ActionResult> DeleteUserWatchlist(int id)
    {
        var userId = GetUserId();

        await _mediator.Send(new DeleteUserWatchlistCommand() { Id = id, UserId = userId });

        return Ok();
    }

    [HttpDelete]
    [SwaggerOperation(OperationId = nameof(DeleteUserWatchlistByProductId))]
    public async Task<ActionResult<UserWatchlistDto>> DeleteUserWatchlistByProductId([FromQuery] int productId)
    {
        var userId = GetUserId();

        await _mediator.Send(new DeleteUserWatchlistByProductIdCommand() { ProductId = productId, UserId = userId });

        return Ok();
    }
}