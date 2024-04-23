using Application.Abstractions;
using Application.App.Users.Responses;
using AuctionApp.Domain.Models;
using MediatR;

namespace Application.App.Users.Commands;

public class DeleteUserCommand : IRequest<UserDto>
{
    public int Id { get; set; }
}

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, UserDto>
{

    private readonly IRepository _repository;

    public DeleteUserCommandHandler(IRepository repository)
    {
        _repository = repository;
    }
    public async Task<UserDto> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _repository.Remove<User>(request.Id);

        await _repository.SaveChanges();

        var userDto = UserDto.FromUser(user);

        return userDto;
    }
}
