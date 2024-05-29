using Application.Common.Abstractions;
using Application.Common.Exceptions;
using AuctionApp.Domain.Models;
using MediatR;

namespace Application.App.Products.Commands;

public class DeleteProductCommand : IRequest
{
    public int Id { get; set; }

    public int UserId { get; set; }
}

public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand>
{
    private readonly IEntityRepository _repository;

    public DeleteProductCommandHandler(IEntityRepository repository)
    {
        _repository = repository;
    }
    public async Task Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _repository.GetById<Product>(request.Id)
            ?? throw new EntityNotFoundException("Product cannot be found");

        if (product.CreatorId != request.UserId)
        {
            throw new InvalidUserException("You do not have permission to modify this data");
        }

        if (product.StartTime <= DateTime.UtcNow + TimeSpan.FromMinutes(5))
        {
            throw new BusinessValidationException("Cannot edit products 5 minutes before its selling start");
        }

        await _repository.Remove<Product>(request.Id);

        await _repository.SaveChanges();
    }
}
