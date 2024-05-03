using Application.Abstractions;
using Application.App.Users.Responses;
using AuctionApp.Domain.Models;
using AutoMapper;
using MediatR;

namespace Application.App.Users.Commands;

public class DeleteUserCommand : IRequest<UserDto>
{
    public int Id { get; set; }
}

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, UserDto>
{

    private readonly IRepository _repository;

    private readonly IMapper _mapper;

    public DeleteUserCommandHandler(IRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    public async Task<UserDto> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _repository.Remove<User>(request.Id);

        await _repository.SaveChanges();

        var userDto = _mapper.Map<User, UserDto>(user);

        return userDto;
    }
}
