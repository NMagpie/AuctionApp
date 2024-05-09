using Application.App.Queries;
using Application.App.UserWatchlists.Commands;
using Application.App.UserWatchlists.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[ApiController]
[Route("[controller]")]
public class UserWatchlistsController : ControllerBase
{

    private readonly IMediator _mediator;

    public UserWatchlistsController(IMediator mediator)
    {
        _mediator = mediator;
    }

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
        await _mediator.Send(new DeleteUserWatchlistCommand() { Id = id });

        return Ok();
    }
}