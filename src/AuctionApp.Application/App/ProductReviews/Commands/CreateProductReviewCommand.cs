using Application.App.ProductReviews.Responses;
using Application.Common.Abstractions;
using Application.Common.Exceptions;
using AuctionApp.Domain.Models;
using AutoMapper;
using Domain.Auth;
using MediatR;

namespace Application.App.ProductReviews.Commands;

public class CreateProductReviewCommand : IRequest<ProductReviewDto>
{
    public int UserId { get; set; }

    public int ProductId { get; set; }

    public string? ReviewText { get; set; }

    public float Rating { get; set; }
}

public class CreateProductReviewCommandHandler : IRequestHandler<CreateProductReviewCommand, ProductReviewDto>
{
    private readonly IEntityRepository _entityRepository;

    private readonly IMapper _mapper;

    public CreateProductReviewCommandHandler(IEntityRepository entityRepository, IMapper mapper)
    {
        _entityRepository = entityRepository;
        _mapper = mapper;
    }

    public async Task<ProductReviewDto> Handle(CreateProductReviewCommand request, CancellationToken cancellationToken)
    {
        var user = await _entityRepository.GetById<User>(request.UserId)
            ?? throw new EntityNotFoundException("User cannot be found");

        var product = await _entityRepository.GetById<Product>(request.ProductId)
            ?? throw new EntityNotFoundException("Product cannot be found");

        if (product.EndTime >= DateTimeOffset.UtcNow)
        {
            throw new BusinessValidationException("Cannot put review: product sell is not finished");
        }

        var productReview = _mapper.Map<CreateProductReviewCommand, ProductReview>(request);

        productReview.DateCreated = DateTimeOffset.UtcNow;

        await _entityRepository.Add(productReview);

        await _entityRepository.SaveChanges();

        var productReviewDto = _mapper.Map<ProductReview, ProductReviewDto>(productReview);

        return productReviewDto;
    }
}
