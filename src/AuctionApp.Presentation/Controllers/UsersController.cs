using Application.App.Queries;
using Application.App.Users.Commands;
using Application.App.Users.Responses;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Common.Abstractions;
using Presentation.Common.Models.Users;

namespace Presentation.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class UsersController : AppBaseController
{
    private readonly IMediator _mediator;

    private readonly IMapper _mapper;

    public UsersController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetUser(int id)
    {
        var userDto = await _mediator.Send(new GetUserByIdQuery() { Id = id });

        return Ok(userDto);
    }

    [HttpGet("current_user")]
    public async Task<ActionResult<UserDto>> GetCurrentUser(int id)
    {
        var userId = GetUserId();

        var userDto = await _mediator.Send(new GetUserByIdQuery() { Id = userId });

        return Ok(userDto);
    }

    [HttpPost("current_user/add-balance")]
    public async Task<ActionResult> AddUserBalance(AddUserBalanceRequest addUserBalanceRequest)
    {
        var userId = GetUserId();

        var userCommand = _mapper.Map<AddUserBalanceRequest, AddUserBalanceCommand>(addUserBalanceRequest);

        userCommand.Id = userId;

        await _mediator.Send(userCommand);

        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<UserDto>> UpdateUser(int id, UpdateUserRequest updateUserRequest)
    {
        var userId = GetUserId();

        var userCommand = _mapper.Map<UpdateUserRequest, UpdateUserCommand>(updateUserRequest);

        userCommand.Id = userId;

        var userDto = await _mediator.Send(userCommand);

        return Ok(userDto);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteUser(int id)
    {
        var userId = GetUserId();

        await _mediator.Send(new DeleteUserCommand() { Id = userId });

        return Ok();
    }
}