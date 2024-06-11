using Application.App.ProductReviews.Responses;
using Application.Common.Abstractions;
using AuctionApp.Application.Common.Models;
using AuctionApp.Domain.Models;
using MediatR;

namespace AuctionApp.Application.App.ProductReviews.Queries;

public class GetProductReviewsOfProductQuery : IRequest<PaginatedResult<ProductReviewDto>>
{
    public int ProductId { get; set; }

    public int PageIndex { get; set; }
}

public class GetProductReviewsOfProductQueryHandler : IRequestHandler<GetProductReviewsOfProductQuery, PaginatedResult<ProductReviewDto>>
{

    private readonly IEntityRepository _repository;

    public GetProductReviewsOfProductQueryHandler(IEntityRepository repository)
    {
        _repository = repository;
    }

    public Task<PaginatedResult<ProductReviewDto>> Handle(GetProductReviewsOfProductQuery query, CancellationToken cancellationToken)
    {
        var request = new PagedRequest
        {
            PageIndex = query.PageIndex,

            PageSize = 10,

            Filter = $"ProductId == {query.ProductId}",

            ColumnNameForSorting = "DateCreated",

            SortDirection = "desc"
        };

        return _repository.GetPagedData<ProductReview, ProductReviewDto>(request, review => review.User);
    }
}
