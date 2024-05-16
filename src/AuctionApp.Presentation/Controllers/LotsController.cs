using Application.App.Lots.Commands;
using Application.App.Lots.Responses;
using Application.App.Queries;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Common.Abstractions;
using Presentation.Common.Models.Lots;
using Presentation.Common.Requests.Lots;

namespace Presentation.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class LotsController : AppBaseController
{
    private readonly IMediator _mediator;

    private readonly IMapper _mapper;

    public LotsController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<ActionResult<LotDto>> GetLot(int id)
    {
        var lotDto = await _mediator.Send(new GetLotByIdQuery() { Id = id });

        return Ok(lotDto);
    }

    [HttpPost]
    public async Task<ActionResult<LotDto>> CreateLot(CreateLotRequest createLotRequest)
    {
        var userId = GetUserId();

        var lotCommand = _mapper.Map<CreateLotRequest, CreateLotCommand>(createLotRequest);

        lotCommand.UserId = userId;

        var lotDto = await _mediator.Send(lotCommand);

        return Ok(lotDto);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<LotDto>> UpdateLot(int id, UpdateLotRequest updateLotRequest)
    {
        var userId = GetUserId();

        var lotCommand = _mapper.Map<UpdateLotRequest, UpdateLotCommand>(updateLotRequest);

        lotCommand.Id = id;

        lotCommand.UserId = userId;

        var lotDto = await _mediator.Send(lotCommand);

        return Ok(lotDto);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteLot(int id)
    {
        var userId = GetUserId();

        await _mediator.Send(new DeleteLotCommand() { Id = id, UserId = userId });

        return Ok();
    }
}