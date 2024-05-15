﻿using Application.Common.Abstractions;
using Application.Common.Exceptions;
using AuctionApp.Domain.Models;
using MediatR;

namespace Application.App.Lots.Commands;

public class DeleteLotCommand : IRequest
{
    public int Id { get; set; }

    public int UserId { get; set; }
}

public class DeleteLotCommandHandler : IRequestHandler<DeleteLotCommand>
{
    private readonly IEntityRepository _repository;

    public DeleteLotCommandHandler(IEntityRepository repository)
    {
        _repository = repository;
    }
    public async Task Handle(DeleteLotCommand request, CancellationToken cancellationToken)
    {
        var lot = await _repository.GetByIdWithInclude<Lot>(request.Id, lot => lot.Auction)
            ?? throw new EntityNotFoundException("Lot cannot be found");

        if (lot.Auction.CreatorId != request.UserId)
        {
            throw new InvalidUserException("You do not have permission to modify this data");
        }

        if (lot.Auction.StartTime <= DateTime.UtcNow + TimeSpan.FromMinutes(5))
        {
            throw new BusinessValidationException("Cannot edit lots of auction 5 minutes before its start");
        }

        if (lot.Auction.Lots.Count <= 1)
        {
            throw new BusinessValidationException("Cannot delete lot: must be at least one present");
        }

        await _repository.Remove<Lot>(request.Id);

        await _repository.SaveChanges();
    }
}
