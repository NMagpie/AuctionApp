using Application.App.Queries;
using Application.App.Users.Commands;
using Application.App.Users.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly ILogger<UsersController> _logger;

    private readonly IMediator _mediator;

    public UsersController(ILogger<UsersController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpGet("{id}")]
    public async Task<UserDto> GetUser(int id)
    {
        var userDto = await _mediator.Send(new GetUserByIdQuery() { Id = id });

        return userDto;
    }

    [HttpPost]
    public async Task<UserDto> CreateUser(CreateUserCommand createUserCommand)
    {
        var userDto = await _mediator.Send(createUserCommand);

        return userDto;
    }

    [HttpPost("add-balance")]
    public async Task AddUserBalance(AddUserBalanceCommand addUserBalanceCommand)
    {
        await _mediator.Send(addUserBalanceCommand);
    }

    [HttpPut("{id}")]
    public async Task<UserDto> UpdateUser(UpdateUserCommand updateUserCommand)
    {
        var userDto = await _mediator.Send(updateUserCommand);

        return userDto;
    }

    [HttpDelete("{id}")]
    public async Task DeleteUser(int id)
    {
        await _mediator.Send(new DeleteUserCommand() { Id = id });
    }
}