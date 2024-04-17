using Application.App.Responses;
using MediatR;

namespace Application.App.Commands.Users;
public class CreateUserCommand : IRequest<UserDto>
{
    public string Username { get; set; }
}
