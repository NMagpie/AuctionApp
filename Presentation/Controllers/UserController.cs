using Application.App.Queries;
using Application.App.Users.Commands;
using Application.App.Users.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;

    private readonly IMediator _mediator;

    public UserController(ILogger<UserController> logger, IMediator mediator)
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

    [HttpPut]
    public async Task<UserDto> UpdateUser(UpdateUserCommand updateUserCommand)
    {
        var userDto = await _mediator.Send(updateUserCommand);

        return userDto;
    }

    [HttpDelete("{id}")]
    public async Task<UserDto> DeleteUser(int id)
    {
        var userDto = await _mediator.Send(new DeleteUserCommand() { Id = id });

        return userDto;
    }
}