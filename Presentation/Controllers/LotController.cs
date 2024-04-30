﻿using Application.App.Lots.Commands;
using Application.App.Lots.Responses;
using Application.App.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[ApiController]
[Route("[controller]")]
public class LotController : ControllerBase
{
    private readonly ILogger<LotController> _logger;

    private readonly IMediator _mediator;

    public LotController(ILogger<LotController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpGet("{id}")]
    public async Task<LotDto> GetLot(int id)
    {
        var lotDto = await _mediator.Send(new GetLotByIdQuery() { Id = id });

        return lotDto;
    }

    [HttpPost]
    public async Task<LotDto> CreateLot(CreateLotCommand createLotCommand)
    {
        var lotDto = await _mediator.Send(createLotCommand);

        return lotDto;
    }

    [HttpPut]
    public async Task<LotDto> UpdateLot(UpdateLotCommand updateLotCommand)
    {
        var lotDto = await _mediator.Send(updateLotCommand);

        return lotDto;
    }

    [HttpDelete("{id}")]
    public async Task<LotDto> DeleteLot(int id)
    {
        var lotDto = await _mediator.Send(new DeleteLotCommand() { Id = id });

        return lotDto;
    }
}