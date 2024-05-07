using Application.Common.Abstractions;
using Application.Common.Exceptions;
using AuctionApp.Domain.Models;
using MediatR;

namespace Application.App.Users.Commands;
public class AddUserBalanceCommand : IRequest
{
    public int Id { get; set; }

    public decimal Amount { get; set; }
}

public class AddUserBalanceCommandHandler : IRequestHandler<AddUserBalanceCommand>
{
    private readonly IRepository _repository;

    private readonly AddUserBalanceCommandValidator _validator;

    public AddUserBalanceCommandHandler(IRepository repository)
    {
        _repository = repository;
        _validator = new AddUserBalanceCommandValidator();
    }

    public async Task Handle(AddUserBalanceCommand request, CancellationToken cancellationToken)
    {
        var user = await _repository.GetById<User>(request.Id)
            ?? throw new EntityNotFoundException("User cannot be found");

        user.Balance += request.Amount;

        await _repository.SaveChanges();
    }
}
