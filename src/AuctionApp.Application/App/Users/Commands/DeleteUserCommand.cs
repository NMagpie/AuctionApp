using Application.Common.Abstractions;
using Domain.Auth;
using MediatR;

namespace Application.App.Users.Commands;

public class DeleteUserCommand : IRequest
{
    public int Id { get; set; }
}

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand>
{

    private readonly IEntityRepository _repository;

    public DeleteUserCommandHandler(IEntityRepository repository)
    {
        _repository = repository;
    }
    public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        await _repository.Remove<User>(request.Id);

        await _repository.SaveChanges();
    }
}
