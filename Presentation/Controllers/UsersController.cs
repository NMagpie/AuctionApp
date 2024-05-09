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
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetUser(int id)
    {
        var userDto = await _mediator.Send(new GetUserByIdQuery() { Id = id });

        return Ok(userDto);
    }

    [HttpPost]
    public async Task<ActionResult<UserDto>> CreateUser(CreateUserCommand createUserCommand)
    {
        var userDto = await _mediator.Send(createUserCommand);

        return Ok(userDto);
    }

    [HttpPost("add-balance")]
    public async Task<ActionResult> AddUserBalance(AddUserBalanceCommand addUserBalanceCommand)
    {
        await _mediator.Send(addUserBalanceCommand);

        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<UserDto>> UpdateUser(UpdateUserCommand updateUserCommand)
    {
        var userDto = await _mediator.Send(updateUserCommand);

        return Ok(userDto);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteUser(int id)
    {
        await _mediator.Send(new DeleteUserCommand() { Id = id });

        return Ok();
    }
}