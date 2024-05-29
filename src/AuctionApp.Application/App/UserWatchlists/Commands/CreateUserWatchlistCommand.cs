using Application.App.UserWatchlists.Responses;
using Application.Common.Abstractions;
using Application.Common.Exceptions;
using AuctionApp.Domain.Models;
using AutoMapper;
using MediatR;

namespace Application.App.UserWatchlists.Commands;

public class CreateUserWatchlistCommand : IRequest<UserWatchlistDto>
{
    public int UserId { get; set; }

    public int ProductId { get; set; }
}

public class CreateUserWatchlistCommandHandler : IRequestHandler<CreateUserWatchlistCommand, UserWatchlistDto>
{
    private readonly IEntityRepository _repository;

    private readonly IUserRepository _userRepository;

    private readonly IMapper _mapper;

    public CreateUserWatchlistCommandHandler(IEntityRepository repository, IUserRepository userRepository, IMapper mapper)
    {
        _repository = repository;
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<UserWatchlistDto> Handle(CreateUserWatchlistCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetById(request.UserId)
            ?? throw new EntityNotFoundException("User cannot be found");

        var product = await _repository.GetById<Product>(request.ProductId)
            ?? throw new EntityNotFoundException("Product cannot be found");

        var userWatchlist = _mapper.Map<CreateUserWatchlistCommand, UserWatchlist>(request);

        await _repository.Add(userWatchlist);

        await _repository.SaveChanges();

        var userWatchlistDto = _mapper.Map<UserWatchlist, UserWatchlistDto>(userWatchlist);

        return userWatchlistDto;
    }
}
