using Application.App.Products.Responses;
using Application.Common.Abstractions;
using Application.Common.Exceptions;
using AuctionApp.Application.Common.Abstractions;
using AuctionApp.Application.Common.Models;
using Domain.Auth;
using MediatR;

namespace AuctionApp.Application.App.Products.Queries;

public class GetProductsUserParticipatedQuery : IPagedRequest, IRequest<PaginatedResult<ProductDto>>
{
    public int UserId { get; set; }

    public int PageIndex { get; set; }

    public int PageSize { get; set; }
}

public class GetProductsUserParticipatedQueryHandler : IRequestHandler<GetProductsUserParticipatedQuery, PaginatedResult<ProductDto>>
{

    private readonly IEntityRepository _entityRepository;

    private readonly IProductQueryRepository _productQueryRepository;

    public GetProductsUserParticipatedQueryHandler(IEntityRepository repository, IProductQueryRepository productQueryRepository)
    {
        _entityRepository = repository;
        _productQueryRepository = productQueryRepository;
    }

    public async Task<PaginatedResult<ProductDto>> Handle(GetProductsUserParticipatedQuery request, CancellationToken cancellationToken)
    {
        var user = await _entityRepository.GetById<User>(request.UserId)
            ?? throw new EntityNotFoundException("User cannot be found");

        return await _productQueryRepository.GetProductsUserParticipated<ProductDto>(request);
    }
}

