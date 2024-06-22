using Application.Common.Abstractions;
using Application.Common.Exceptions;
using AuctionApp.Domain.Models;
using MediatR;

namespace AuctionApp.Application.App.SearchQueries.Commands;

public class DeleteSearchRecordCommand : IRequest
{
    public int Id { get; set; }

    public int UserId { get; set; }
}

public class DeleteSearchRecordCommandHandler : IRequestHandler<DeleteSearchRecordCommand>
{
    private readonly IEntityRepository _repository;

    public DeleteSearchRecordCommandHandler(IEntityRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(DeleteSearchRecordCommand request, CancellationToken cancellationToken)
    {
        var searchRecord = await _repository.GetById<SearchRecord>(request.Id)
            ?? throw new EntityNotFoundException("Search record cannot be found");

        if (searchRecord.UserId != request.UserId)
        {
            throw new InvalidUserException("You do not have permission to modify this data");
        }

        await _repository.Remove<SearchRecord>(request.Id);

        await _repository.SaveChanges();
    }
}