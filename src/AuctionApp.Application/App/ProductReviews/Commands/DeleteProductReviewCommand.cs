using Application.Common.Abstractions;
using Application.Common.Exceptions;
using AuctionApp.Domain.Models;
using MediatR;

namespace Application.App.ProductReviews.Commands;

public class DeleteProductReviewCommand : IRequest
{
    public int Id { get; set; }

    public int UserId { get; set; }
}

public class DeleteProductReviewCommandHandler : IRequestHandler<DeleteProductReviewCommand>
{
    private readonly IEntityRepository _repository;

    public DeleteProductReviewCommandHandler(IEntityRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(DeleteProductReviewCommand request, CancellationToken cancellationToken)
    {
        var productReview = await _repository.GetById<ProductReview>(request.Id)
            ?? throw new EntityNotFoundException("Product Review cannot be found");

        if (productReview.UserId != request.UserId)
        {
            throw new InvalidUserException("You do not have permission to modify this data");
        }

        await _repository.Remove<ProductReview>(request.Id);

        await _repository.SaveChanges();
    }
}
