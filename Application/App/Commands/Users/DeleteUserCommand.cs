using Application.App.Responses;
using MediatR;

namespace Application.App.Commands.Users;
public class DeleteUserCommand : IRequest<UserDto>
{
    public int Id { get; set; }
}

