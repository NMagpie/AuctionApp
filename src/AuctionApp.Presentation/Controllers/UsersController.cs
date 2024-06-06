using Application.App.Queries;
using Application.App.Users.Responses;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Presentation.Common.Abstractions;
using Swashbuckle.AspNetCore.Annotations;

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

    [HttpGet("{id}")]
    [SwaggerOperation(OperationId = nameof(GetUser))]
    public async Task<ActionResult<UserDto>> GetUser(int id)
    {
        var userDto = await _mediator.Send(new GetUserByIdQuery() { Id = id });

        return Ok(userDto);
    }
}