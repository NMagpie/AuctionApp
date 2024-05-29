using Application.App.ProductReviews.Responses;
using Application.Common.Abstractions;
using Application.Common.Exceptions;
using AuctionApp.Domain.Models;
using AutoMapper;
using MediatR;

namespace Application.App.ProductReviews.Commands;

public class UpdateProductReviewCommand : IRequest<ProductReviewDto>
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string ReviewText { get; set; }

    public float Rating { get; set; }
}

public class UpdateProductReviewCommandHandler : IRequestHandler<UpdateProductReviewCommand, ProductReviewDto>
{
    private readonly IEntityRepository _repository;

    private readonly IMapper _mapper;

    public UpdateProductReviewCommandHandler(IEntityRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ProductReviewDto> Handle(UpdateProductReviewCommand request, CancellationToken cancellationToken)
    {
        var productReview = await _repository.GetById<ProductReview>(request.Id)
            ?? throw new EntityNotFoundException("ProductReview cannot be found");

        if (productReview.UserId != request.UserId)
        {
            throw new InvalidUserException("You do not have permission to modify this data");
        }

        _mapper.Map(request, productReview);

        await _repository.SaveChanges();

        var productReviewDto = _mapper.Map<ProductReview, ProductReviewDto>(productReview);

        return productReviewDto;
    }
}
