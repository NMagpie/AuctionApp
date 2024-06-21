using Application.App.Products.Responses;
using Application.App.Queries;
using Application.App.Users.Responses;
using AuctionApp.Application.App.Products.Queries;
using AuctionApp.Application.Common.Models;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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

    [HttpGet("{id}/participated")]
    [AllowAnonymous]
    [SwaggerOperation(OperationId = nameof(GetUserParticipatedProducts))]
    public async Task<ActionResult<PaginatedResult<ProductDto>>> GetUserParticipatedProducts(int id, [FromQuery] int PageIndex, [FromQuery] int PageSize)
    {
        var result = await _mediator.Send(new GetProductsUserParticipatedQuery()
        {
            UserId = id,
            PageIndex = PageIndex,
            PageSize = PageSize
        });

        return Ok(result);
    }
}