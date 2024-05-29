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
[Route("Users/Me")]
[Authorize]
public class CurrentUserController : AppBaseController
{
    private readonly IMediator _mediator;

    private readonly IMapper _mapper;

    public CurrentUserController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpGet()]
    public async Task<ActionResult<CurrentUserDto>> GetCurrentUser()
    {
        var userId = GetUserId();

        var userDto = await _mediator.Send(new GetCurrentUserQuery() { Id = userId });

        return Ok(userDto);
    }

    [HttpPost("add-balance")]
    public async Task<ActionResult> AddUserBalance(AddUserBalanceRequest addUserBalanceRequest)
    {
        var userId = GetUserId();

        var userCommand = _mapper.Map<AddUserBalanceRequest, AddUserBalanceCommand>(addUserBalanceRequest);

        userCommand.Id = userId;

        await _mediator.Send(userCommand);

        return Ok();
    }

    [HttpPut()]
    public async Task<ActionResult<CurrentUserDto>> UpdateUser(UpdateUserRequest updateUserRequest)
    {
        var userId = GetUserId();

        var userCommand = _mapper.Map<UpdateUserRequest, UpdateUserCommand>(updateUserRequest);

        userCommand.Id = userId;

        var userDto = await _mediator.Send(userCommand);

        return Ok(userDto);
    }

    [HttpDelete()]
    public async Task<ActionResult> DeleteUser()
    {
        var userId = GetUserId();

        await _mediator.Send(new DeleteUserCommand() { Id = userId });

        return Ok();
    }
}