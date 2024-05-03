﻿using Application.Abstractions;
using Application.App.UserWatchlists.Responses;
using AuctionApp.Domain.Models;
using AutoMapper;
using MediatR;

namespace Application.App.UserWatchlists.Commands;

public class CreateUserWatchlistCommand : IRequest<UserWatchlistDto>
{
    public int UserId { get; set; }

    public int AuctionId { get; set; }
}

public class CreateUserWatchlistCommandHandler : IRequestHandler<CreateUserWatchlistCommand, UserWatchlistDto>
{
    private readonly IRepository _repository;

    private readonly CreateUserWatchlistCommandValidator _validator;

    private readonly IMapper _mapper;

    public CreateUserWatchlistCommandHandler(IRepository repository, IMapper mapper)
    {
        _repository = repository;
        _validator = new CreateUserWatchlistCommandValidator();
        _mapper = mapper;
    }

    public async Task<UserWatchlistDto> Handle(CreateUserWatchlistCommand request, CancellationToken cancellationToken)
    {
        _validator.Validate(request);

        var user = await _repository.GetById<User>(request.UserId)
            ?? throw new ArgumentNullException("User cannot be found");

        var auction = await _repository.GetById<Auction>(request.AuctionId)
            ?? throw new ArgumentNullException("Auction cannot be found");

        var userWatchlist = _mapper.Map<CreateUserWatchlistCommand, UserWatchlist>(request);

        await _repository.Add(userWatchlist);

        await _repository.SaveChanges();

        var userWatchlistDto = _mapper.Map<UserWatchlist, UserWatchlistDto>(userWatchlist);

        return userWatchlistDto;
    }
}
