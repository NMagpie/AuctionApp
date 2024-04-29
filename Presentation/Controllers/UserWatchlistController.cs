using Application.App.Queries;
using Application.App.UserWatchlists.Commands;
using Application.App.UserWatchlists.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[ApiController]
[Route("[controller]")]
public class UserWatchlistController : ControllerBase
{
    private readonly ILogger<UserWatchlistController> _logger;

    private readonly IMediator _mediator;

    public UserWatchlistController(ILogger<UserWatchlistController> logger, IMediator mediator)
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
    public async Task<UserWatchlistDto> DeleteUserWatchlist(int id)
    {
        var userWatchlistDto = await _mediator.Send(new DeleteUserWatchlistCommand() { Id = id });

        return userWatchlistDto;
    }
}