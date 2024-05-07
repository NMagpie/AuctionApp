using Application.Common.Abstractions;
using AuctionApp.Domain.Models;
using AutoMapper;
using MediatR;

namespace Application.App.UserWatchlists.Commands;

public class DeleteUserWatchlistCommand : IRequest
{
    public int Id { get; set; }
}

public class DeleteUserWatchlistCommandHandler : IRequestHandler<DeleteUserWatchlistCommand>
{
    private readonly IRepository _repository;

    private readonly IMapper _mapper;

    public DeleteUserWatchlistCommandHandler(IRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task Handle(DeleteUserWatchlistCommand request, CancellationToken cancellationToken)
    {
        await _repository.Remove<UserWatchlist>(request.Id);

        await _repository.SaveChanges();
    }
}
