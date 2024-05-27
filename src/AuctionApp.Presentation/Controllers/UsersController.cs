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
}