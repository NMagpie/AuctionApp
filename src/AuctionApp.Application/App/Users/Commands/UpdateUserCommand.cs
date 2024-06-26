﻿using Application.App.Users.Responses;
using Application.Common.Abstractions;
using Application.Common.Exceptions;
using AutoMapper;
using Domain.Auth;
using MediatR;

namespace Application.App.Users.Commands;

public class UpdateUserCommand : IRequest<CurrentUserDto>
{
    public int Id { get; set; }

    public string UserName { get; set; }

    public string Email { get; set; }

    public string PhoneNumber { get; set; }
}

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, CurrentUserDto>
{

    private readonly IEntityRepository _repository;

    private readonly IMapper _mapper;

    public UpdateUserCommandHandler(IEntityRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<CurrentUserDto> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _repository.GetById<User>(request.Id)
            ?? throw new EntityNotFoundException("User cannot be found");

        _mapper.Map(request, user);

        await _repository.SaveChanges();

        var userDto = _mapper.Map<User, CurrentUserDto>(user);

        return userDto;
    }
}
