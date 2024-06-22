using Application.Common.Abstractions;
using AuctionApp.Domain.Models;
using MediatR;

namespace AuctionApp.Application.App.SearchQueries.Commands;

public class DeleteAllSearchRecordsCommand : IRequest
{
    public int UserId { get; set; }
}

public class DeleteAllSearchRecordsCommandHandler : IRequestHandler<DeleteAllSearchRecordsCommand>
{
    private readonly IEntityRepository _repository;

    public DeleteAllSearchRecordsCommandHandler(IEntityRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(DeleteAllSearchRecordsCommand request, CancellationToken cancellationToken)
    {
        await _repository.RemoveByPredicate<SearchRecord>(r => r.UserId == request.UserId);

        await _repository.SaveChanges();
    }
}