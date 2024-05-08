using Application.Common.Abstractions;
using AuctionApp.Domain.Models;
using MediatR;

namespace Application.App.Users.Commands;

public class DeleteUserCommand : IRequest
{
    public int Id { get; set; }
}

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand>
{

    private readonly IRepository _repository;

    public DeleteUserCommandHandler(IRepository repository)
    {
        _repository = repository;
    }
    public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        await _repository.Remove<User>(request.Id);

        await _repository.SaveChanges();
    }
}
