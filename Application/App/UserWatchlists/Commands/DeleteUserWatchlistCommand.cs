using Application.Abstractions;
using Application.App.UserWatchlists.Responses;
using AuctionApp.Domain.Models;
using AutoMapper;
using MediatR;

namespace Application.App.UserWatchlists.Commands;

public class DeleteUserWatchlistCommand : IRequest<UserWatchlistDto>
{
    public int Id { get; set; }
}

public class DeleteUserWatchlistCommandHandler : IRequestHandler<DeleteUserWatchlistCommand, UserWatchlistDto>
{
    private readonly IRepository _repository;

    private readonly IMapper _mapper;

    public DeleteUserWatchlistCommandHandler(IRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<UserWatchlistDto> Handle(DeleteUserWatchlistCommand request, CancellationToken cancellationToken)
    {
        var userWatchlist = await _repository.Remove<UserWatchlist>(request.Id);

        await _repository.SaveChanges();

        var userWatchlistDto = _mapper.Map<UserWatchlist, UserWatchlistDto>(userWatchlist);

        return userWatchlistDto;
    }
}
