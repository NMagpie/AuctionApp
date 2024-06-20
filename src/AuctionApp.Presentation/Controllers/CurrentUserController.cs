using Application.App.Products.Responses;
using Application.App.Queries;
using Application.App.Users.Commands;
using Application.App.Users.Responses;
using AuctionApp.Application.App.Products.Queries;
using AuctionApp.Application.Common.Models;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Common.Abstractions;
using Presentation.Common.Models.Users;
using Swashbuckle.AspNetCore.Annotations;

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

    [HttpGet]
    [SwaggerOperation(OperationId = nameof(GetCurrentUser))]
    public async Task<ActionResult<CurrentUserDto>> GetCurrentUser()
    {
        var userId = GetUserId();

        var userDto = await _mediator.Send(new GetCurrentUserQuery() { Id = userId });

        return Ok(userDto);
    }

    [HttpPost("add-balance")]
    [SwaggerOperation(OperationId = nameof(AddUserBalance))]
    public async Task<ActionResult<CurrentUserDto>> AddUserBalance(AddUserBalanceRequest addUserBalanceRequest)
    {
        var userId = GetUserId();

        var userCommand = _mapper.Map<AddUserBalanceRequest, AddUserBalanceCommand>(addUserBalanceRequest);

        userCommand.Id = userId;

        var result = await _mediator.Send(userCommand);

        return Ok(result);
    }

    [HttpGet("/watchlist")]
    [SwaggerOperation(OperationId = nameof(GetUserWatchlist))]
    public async Task<ActionResult<PaginatedResult<ProductDto>>> GetUserWatchlist([FromQuery] int PageIndex, [FromQuery] int PageSize)
    {
        var userId = GetUserId();

        var result = await _mediator.Send(new GetUserWatchlistQuery()
        {
            UserId = userId,
            PageIndex = PageIndex,
            PageSize = PageSize
        });

        return Ok(result);
    }

    [HttpPut]
    [SwaggerOperation(OperationId = nameof(UpdateUser))]
    public async Task<ActionResult<CurrentUserDto>> UpdateUser(UpdateUserRequest updateUserRequest)
    {
        var userId = GetUserId();

        var userCommand = _mapper.Map<UpdateUserRequest, UpdateUserCommand>(updateUserRequest);

        userCommand.Id = userId;

        var userDto = await _mediator.Send(userCommand);

        return Ok(userDto);
    }

    [HttpDelete]
    [SwaggerOperation(OperationId = nameof(DeleteUser))]
    public async Task<ActionResult> DeleteUser()
    {
        var userId = GetUserId();

        await _mediator.Send(new DeleteUserCommand() { Id = userId });

        return Ok();
    }
}