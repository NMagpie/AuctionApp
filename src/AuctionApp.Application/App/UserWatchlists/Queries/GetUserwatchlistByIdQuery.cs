using Application.App.UserWatchlists.Responses;
using Application.Common.Abstractions;
using Application.Common.Exceptions;
using AuctionApp.Domain.Models;
using AutoMapper;
using MediatR;

namespace Application.App.Queries;
public class GetUserWatchlistByIdQuery : IRequest<UserWatchlistDto>
{
    public int Id { get; set; }

    public int UserId { get; set; }
}

public class GetUserWatchlistByIdQueryHandler : IRequestHandler<GetUserWatchlistByIdQuery, UserWatchlistDto>
{
    private readonly IEntityRepository _repository;

    private readonly IMapper _mapper;

    public GetUserWatchlistByIdQueryHandler(IEntityRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<UserWatchlistDto> Handle(GetUserWatchlistByIdQuery request, CancellationToken cancellationToken)
    {
        var userWatchlist = await _repository.GetById<UserWatchlist>(request.Id)
            ?? throw new EntityNotFoundException("User watchlist cannot be found");

        if (userWatchlist.UserId != request.UserId)
        {
            throw new InvalidUserException("You do not have permission to access this data");
        }

        var userWatchlistDto = _mapper.Map<UserWatchlist, UserWatchlistDto>(userWatchlist);

        return userWatchlistDto;
    }
}