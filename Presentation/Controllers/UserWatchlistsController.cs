using Application.App.Queries;
using Application.App.UserWatchlists.Commands;
using Application.App.UserWatchlists.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Common.Abstractions;

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

    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<ActionResult<UserWatchlistDto>> GetUserWatchlist(int id)
    {
        var userWatchlistDto = await _mediator.Send(new GetUserWatchlistByIdQuery() { Id = id });

        return Ok(userWatchlistDto);
    }

    [HttpPost]
    public async Task<ActionResult<UserWatchlistDto>> CreateUserWatchlist(CreateUserWatchlistCommand createUserWatchlistCommand)
    {
        var userWatchlistDto = await _mediator.Send(createUserWatchlistCommand);

        return Ok(userWatchlistDto);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteUserWatchlist(int id)
    {
        var userId = GetUserId();

        await _mediator.Send(new DeleteUserWatchlistCommand() { Id = id, UserId = userId });

        return Ok();
    }
}