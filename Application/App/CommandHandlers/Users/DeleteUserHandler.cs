using Application.Abstractions;
using Application.App.Commands.Users;
using Application.App.Responses;
using AuctionApp.Domain.Models;
using MediatR;

namespace Application.App.CommandHandlers.Users;
public class DeleteUserHandler : IRequestHandler<DeleteUserCommand, UserDto>
{

    private readonly IUnitOfWork _unitofWork;

    public DeleteUserHandler(IUnitOfWork unitOfWork)
    {
        _unitofWork = unitOfWork;
    }
    public async Task<UserDto> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _unitofWork.Repository.Remove<User>(request.Id);

        await _unitofWork.SaveChanges();

        var userDto = UserDto.FromUser(user);

        return userDto;
    }
}
