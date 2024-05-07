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
    private readonly ILogger<UserWatchlistsController> _logger;

    private readonly IMediator _mediator;

    public UserWatchlistsController(ILogger<UserWatchlistsController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpGet("{id}")]
    public async Task<UserWatchlistDto> GetUserWatchlist(int id)
    {
        var userWatchlistDto = await _mediator.Send(new GetUserWatchlistByIdQuery() { Id = id });

        return userWatchlistDto;
    }

    [HttpPost]
    public async Task<UserWatchlistDto> CreateUserWatchlist(CreateUserWatchlistCommand createUserWatchlistCommand)
    {
        var userWatchlistDto = await _mediator.Send(createUserWatchlistCommand);

        return userWatchlistDto;
    }

    [HttpDelete("{id}")]
    public async Task DeleteUserWatchlist(int id)
    {
        await _mediator.Send(new DeleteUserWatchlistCommand() { Id = id });
    }
}