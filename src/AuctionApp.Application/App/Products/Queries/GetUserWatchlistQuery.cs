using Application.App.Products.Responses;
using Application.Common.Abstractions;
using AuctionApp.Domain.Models;
using AutoMapper;
using MediatR;

namespace AuctionApp.Application.App.Products.Queries;

public class GetUserWatchlistQuery : IRequest<List<ProductDto>>
{
    public int UserId { get; set; }
}

public class GetUserWatchlistQueryHandler : IRequestHandler<GetUserWatchlistQuery, List<ProductDto>>
{

    private readonly IEntityRepository _repository;

    private readonly IMapper _mapper;

    public GetUserWatchlistQueryHandler(IEntityRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<List<ProductDto>> Handle(GetUserWatchlistQuery request, CancellationToken cancellationToken)
    {
        var watchlist = await _repository.GetByPredicate<UserWatchlist>(w => w.UserId == request.UserId);

        var products = await _repository.GetByPredicate<Product>(p => watchlist.Select(w => w.ProductId).Contains(p.Id));

        var result = _mapper.Map<List<ProductDto>>(products);

        return result;
    }
}
