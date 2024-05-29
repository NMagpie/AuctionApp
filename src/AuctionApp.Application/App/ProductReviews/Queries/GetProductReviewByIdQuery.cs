using Application.App.ProductReviews.Responses;
using Application.Common.Abstractions;
using Application.Common.Exceptions;
using AuctionApp.Domain.Models;
using AutoMapper;
using MediatR;

namespace Application.App.Queries;
public class GetProductReviewByIdQuery : IRequest<ProductReviewDto>
{
    public int Id { get; set; }
}

public class GetProductReviewByIdQueryHandler : IRequestHandler<GetProductReviewByIdQuery, ProductReviewDto>
{
    private readonly IEntityRepository _repository;

    private readonly IMapper _mapper;

    public GetProductReviewByIdQueryHandler(IEntityRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ProductReviewDto> Handle(GetProductReviewByIdQuery request, CancellationToken cancellationToken)
    {
        var productReview = await _repository.GetById<ProductReview>(request.Id)
            ?? throw new EntityNotFoundException("Product Review cannot be found");

        var productReviewDto = _mapper.Map<ProductReview, ProductReviewDto>(productReview);

        return productReviewDto;
    }
}