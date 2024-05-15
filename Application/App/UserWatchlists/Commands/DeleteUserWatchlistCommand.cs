using Application.Common.Abstractions;
using Application.Common.Exceptions;
using AuctionApp.Domain.Models;
using MediatR;

namespace Application.App.UserWatchlists.Commands;

public class DeleteUserWatchlistCommand : IRequest
{
    public int Id { get; set; }

    public int UserId { get; set; }
}

public class DeleteUserWatchlistCommandHandler : IRequestHandler<DeleteUserWatchlistCommand>
{
    private readonly IEntityRepository _repository;

    public DeleteUserWatchlistCommandHandler(IEntityRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(DeleteUserWatchlistCommand request, CancellationToken cancellationToken)
    {
        var userWatchlist = await _repository.GetById<UserWatchlist>(request.Id)
            ?? throw new EntityNotFoundException("User Watchlist cannot be found");

        if (userWatchlist.UserId != request.UserId)
        {
            throw new InvalidUserException("You do not have permission to modify this data");
        }

        await _repository.Remove<UserWatchlist>(request.Id);

        await _repository.SaveChanges();
    }
}
